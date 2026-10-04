using System.Net;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Project.Middlewares;

namespace Project.Tests;

public class ExceptionMiddlewareTests
{
    private static async Task<IHost> StartAsync(string environment, Func<HttpContext, Task> endpoint) =>
        await new HostBuilder()
            .UseEnvironment(environment)
            .ConfigureWebHost(web => web
                .UseTestServer()
                .Configure(app =>
                {
                    app.UseMiddleware<ExceptionMiddleware>();
                    app.Run(new RequestDelegate(endpoint));
                }))
            .StartAsync();

    private static Task Boom(HttpContext _) => throw new InvalidOperationException("secret connection detail");

    [Fact]
    public async Task A_normal_request_passes_through()
    {
        using var host = await StartAsync("Production", ctx => ctx.Response.WriteAsync("ok"));

        var response = await host.GetTestClient().GetAsync("/");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).Should().Be("ok");
    }

    [Fact]
    public async Task An_exception_becomes_500_with_a_json_content_type()
    {
        using var host = await StartAsync("Production", Boom);

        var response = await host.GetTestClient().GetAsync("/");

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        response.Content.Headers.ContentType!.MediaType.Should().Be("application/json");
    }

    [Fact]
    public async Task The_body_is_real_JSON_with_an_error_property()
    {
        using var host = await StartAsync("Production", Boom);

        var body = await (await host.GetTestClient().GetAsync("/")).Content.ReadAsStringAsync();

        // The old code wrote the C# text "{ error = ... }", which is not JSON.
        using var json = JsonDocument.Parse(body);
        json.RootElement.GetProperty("error").GetString().Should().Be("Internal Server Error");
    }

    [Fact]
    public async Task Production_does_not_leak_the_message_or_the_stack_trace()
    {
        using var host = await StartAsync("Production", Boom);

        var body = await (await host.GetTestClient().GetAsync("/")).Content.ReadAsStringAsync();

        body.Should().NotContain("secret connection detail").And.NotContain("   at ");
    }

    [Fact]
    public async Task Development_includes_the_message_and_stack_trace()
    {
        using var host = await StartAsync("Development", Boom);

        var body = await (await host.GetTestClient().GetAsync("/")).Content.ReadAsStringAsync();

        using var json = JsonDocument.Parse(body);
        var error = json.RootElement.GetProperty("error").GetString()!;
        error.Should().Contain("secret connection detail").And.Contain("   at ");
    }

    [Fact]
    public async Task Headers_set_before_the_failure_are_cleared()
    {
        using var host = await StartAsync("Production", ctx =>
        {
            ctx.Response.Headers["X-Half-Done"] = "1";
            throw new InvalidOperationException("late");
        });

        var response = await host.GetTestClient().GetAsync("/");

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        response.Headers.Contains("X-Half-Done").Should().BeFalse();
    }
}
