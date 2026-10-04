using System.Net;
using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using NSubstitute;
using Project.DataAccess;
using Project.Models;

namespace Project.Tests;

public class HttpTests
{
    private static HttpClient Client(AppFactory f) =>
        f.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

    private static async Task<string> TokenAsync(HttpClient client, string url)
    {
        var html = await client.GetStringAsync(url);
        var m = Regex.Match(html, "name=\"__RequestVerificationToken\"[^>]*value=\"([^\"]+)\"");
        m.Success.Should().BeTrue("the form must carry an anti-forgery token");
        return m.Groups[1].Value;
    }

    private static Dictionary<string, string> Form(string token, Action<Dictionary<string, string>>? change = null)
    {
        var form = new Dictionary<string, string>
        {
            ["__RequestVerificationToken"] = token,
            ["EmployeeId"] = "5", ["LastName"] = "Smith", ["FirstName"] = "John", ["MiddleName"] = "Paul",
            ["HireDate"] = "2020-03-04", ["Salary"] = "1500", ["DepartmentId"] = "2", ["PositionId"] = "3", ["CompanyId"] = "1",
        };
        change?.Invoke(form);
        return form;
    }

    // ---- Home ----

    [Fact]
    public async Task Home_shows_the_company()
    {
        using var f = new AppFactory();

        var html = await Client(f).GetStringAsync("/");

        html.Should().Contain("Acme Corp").And.Contain("1 Main St");
    }

    [Fact]
    public async Task Home_without_a_company_row_shows_a_friendly_message()
    {
        using var f = new AppFactory();
        f.Data.QueryAsync<Company>(Arg.Any<string>(), Arg.Any<SqlParameter[]>()).Returns(_ => new List<Company>());

        var response = await Client(f).GetAsync("/");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).Should().Contain("Company information not found");
    }

    // ---- Employees ----

    [Fact]
    public async Task Employee_list_shows_the_employees()
    {
        using var f = new AppFactory();

        var html = await Client(f).GetStringAsync("/Employees");

        html.Should().Contain("Smith").And.Contain("John");
    }

    [Fact]
    public async Task Employee_list_passes_the_filters_to_the_database_as_parameters()
    {
        using var f = new AppFactory();

        (await Client(f).GetAsync("/Employees?departmentId=2&positionId=3&lastName=Smi")).StatusCode.Should().Be(HttpStatusCode.OK);

        await f.Data.Received().QueryAsync<Employee>(
            Arg.Is<string>(s => s.Contains("department_id = @departmentId") && s.Contains("position_id = @positionId") && s.Contains("last_name LIKE") && !s.Contains("Smi")),
            Arg.Is<SqlParameter[]>(ps => ps.Any(p => p.ParameterName == "@lastName" && (string)p.Value == "Smi")));
    }

    [Fact]
    public async Task Searching_by_last_name_does_not_filter_on_the_middle_name()
    {
        using var f = new AppFactory();

        await Client(f).GetAsync("/Employees?lastName=Smith");

        await f.Data.Received().QueryAsync<Employee>(Arg.Is<string>(s => s.Contains("last_name LIKE") && !s.Contains("middle_name LIKE")), Arg.Any<SqlParameter[]>());
    }

    [Fact]
    public async Task An_injection_attempt_stays_out_of_the_sql_text()
    {
        using var f = new AppFactory();

        const string evil = "x'; DROP TABLE employees;--";

        var response = await Client(f).GetAsync("/Employees?lastName=" + Uri.EscapeDataString(evil));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await f.Data.Received().QueryAsync<Employee>(
            Arg.Is<string>(s => s.Contains("last_name LIKE") && !s.Contains("DROP")),
            Arg.Is<SqlParameter[]>(ps => ps.Any(p => p.ParameterName == "@lastName" && (string)p.Value == evil)));
    }

    // ---- Details ----

    [Fact]
    public async Task Details_of_an_unknown_employee_is_404()
    {
        using var f = new AppFactory();
        f.Data.QueryAsync<Employee>(SqlQueries.GetEmployeeById, Arg.Any<SqlParameter[]>()).Returns(_ => new List<Employee>());

        (await Client(f).GetAsync("/Employees/Details/999")).StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Details_shows_the_current_values()
    {
        using var f = new AppFactory();
        f.Data.QueryAsync<Employee>(SqlQueries.GetEmployeeById, Arg.Any<SqlParameter[]>()).Returns(_ => new List<Employee> { AppFactory.Sample() });

        var html = await Client(f).GetStringAsync("/Employees/Details/5");

        html.Should().Contain("value=\"Smith\"").And.Contain("value=\"John\"");
    }

    [Fact]
    public async Task Saving_without_an_anti_forgery_token_is_rejected()
    {
        using var f = new AppFactory();
        var client = Client(f);

        var form = Form("missing");
        form.Remove("__RequestVerificationToken");
        var response = await client.PostAsync("/Employees/Details", new FormUrlEncodedContent(form));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await f.Data.DidNotReceive().ExecuteNonQueryAsync(Arg.Any<string>(), Arg.Any<SqlParameter[]>());
    }

    [Fact]
    public async Task Saving_a_valid_employee_updates_and_redirects_to_the_list()
    {
        using var f = new AppFactory();
        f.Data.QueryAsync<Employee>(SqlQueries.GetEmployeeById, Arg.Any<SqlParameter[]>()).Returns(_ => new List<Employee> { AppFactory.Sample() });
        var client = Client(f);
        var token = await TokenAsync(client, "/Employees/Details/5");

        var response = await client.PostAsync("/Employees/Details", new FormUrlEncodedContent(Form(token)));

        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location!.OriginalString.Should().Be("/Employees");
        await f.Data.Received(1).ExecuteNonQueryAsync(SqlQueries.UpdateEmployee, Arg.Any<SqlParameter[]>());
    }

    [Fact]
    public async Task Saving_an_employee_without_a_middle_name_works()
    {
        using var f = new AppFactory();
        f.Data.QueryAsync<Employee>(SqlQueries.GetEmployeeById, Arg.Any<SqlParameter[]>()).Returns(_ => new List<Employee> { AppFactory.Sample() });
        var client = Client(f);
        var token = await TokenAsync(client, "/Employees/Details/5");

        var response = await client.PostAsync("/Employees/Details", new FormUrlEncodedContent(Form(token, form => form.Remove("MiddleName"))));

        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        await f.Data.Received(1).ExecuteNonQueryAsync(SqlQueries.UpdateEmployee,
            Arg.Is<SqlParameter[]>(ps => ps.Single(p => p.ParameterName == "@middleName").Value == DBNull.Value));
    }

    [Fact]
    public async Task Saving_an_invalid_employee_shows_the_form_again_and_stores_nothing()
    {
        using var f = new AppFactory();
        f.Data.QueryAsync<Employee>(SqlQueries.GetEmployeeById, Arg.Any<SqlParameter[]>()).Returns(_ => new List<Employee> { AppFactory.Sample() });
        var client = Client(f);
        var token = await TokenAsync(client, "/Employees/Details/5");

        var response = await client.PostAsync("/Employees/Details", new FormUrlEncodedContent(Form(token, form => form["LastName"] = "")));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).Should().Contain("Last name is required");
        await f.Data.DidNotReceive().ExecuteNonQueryAsync(Arg.Any<string>(), Arg.Any<SqlParameter[]>());
    }

    [Fact]
    public async Task Saving_a_salary_out_of_range_is_rejected()
    {
        using var f = new AppFactory();
        f.Data.QueryAsync<Employee>(SqlQueries.GetEmployeeById, Arg.Any<SqlParameter[]>()).Returns(_ => new List<Employee> { AppFactory.Sample() });
        var client = Client(f);
        var token = await TokenAsync(client, "/Employees/Details/5");

        var response = await client.PostAsync("/Employees/Details", new FormUrlEncodedContent(Form(token, form => form["Salary"] = "2000000")));

        (await response.Content.ReadAsStringAsync()).Should().Contain("Salary must be between");
        await f.Data.DidNotReceive().ExecuteNonQueryAsync(Arg.Any<string>(), Arg.Any<SqlParameter[]>());
    }

    [Fact]
    public async Task Saving_an_employee_that_no_longer_exists_is_404()
    {
        using var f = new AppFactory();
        f.Data.QueryAsync<Employee>(SqlQueries.GetEmployeeById, Arg.Any<SqlParameter[]>()).Returns(_ => new List<Employee> { AppFactory.Sample() });
        var client = Client(f);
        var token = await TokenAsync(client, "/Employees/Details/5");
        f.Data.ExecuteNonQueryAsync(Arg.Any<string>(), Arg.Any<SqlParameter[]>()).Returns(0);

        var response = await client.PostAsync("/Employees/Details", new FormUrlEncodedContent(Form(token)));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ---- Salary report ----

    [Fact]
    public async Task Salary_report_lists_employees_without_a_total_when_no_department_is_chosen()
    {
        using var f = new AppFactory();

        var response = await Client(f).GetAsync("/SalaryReport");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).Should().Contain("Smith");
        await f.Data.DidNotReceive().ScalarAsync(Arg.Any<string>(), Arg.Any<SqlParameter[]>());
    }

    [Fact]
    public async Task Salary_report_shows_the_department_total()
    {
        using var f = new AppFactory();
        f.Data.ScalarAsync(Arg.Any<string>(), Arg.Any<SqlParameter[]>()).Returns((object?)98765.43m);

        var html = await Client(f).GetStringAsync("/SalaryReport?departmentId=2");

        html.Should().Contain("98").And.Contain("765");
    }

    [Fact]
    public async Task Export_returns_a_text_attachment_with_the_report()
    {
        using var f = new AppFactory();

        var response = await Client(f).GetAsync("/SalaryReport/ExportToTxt?departmentId=2");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType!.MediaType.Should().Be("text/plain");
        response.Content.Headers.ContentDisposition!.FileName.Should().Be("salary_report.txt");
        var text = await response.Content.ReadAsStringAsync();
        text.Should().StartWith("Employee ID\tLast Name");
        text.Should().Contain("5\tSmith\tJohn\tPaul\t1500.00\t2\t3");
    }

    // ---- Errors ----

    [Fact]
    public async Task A_database_failure_is_a_json_500_without_details_in_production()
    {
        using var f = new AppFactory();
        f.Data.QueryAsync<Employee>(Arg.Any<string>(), Arg.Any<SqlParameter[]>()).Returns<Task<List<Employee>>>(_ => throw new InvalidOperationException("Server=prod;Password=secret"));

        var response = await Client(f).GetAsync("/Employees");

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        response.Content.Headers.ContentType!.MediaType.Should().Be("application/json");
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("\"error\"").And.NotContain("secret");
    }
}
