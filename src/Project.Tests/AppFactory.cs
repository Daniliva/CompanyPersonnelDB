using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.SqlClient;
using NSubstitute;
using Project.DataAccess;
using Project.Models;

namespace Project.Tests;

/// <summary>The real application with the database replaced by a substitute (no SQL Server needed).</summary>
public sealed class AppFactory : WebApplicationFactory<Program>
{
    public IDataAccess Data { get; } = Substitute.For<IDataAccess>();

    public string Environment { get; init; } = "Production";

    public AppFactory()
    {
        Data.QueryAsync<Company>(Arg.Any<string>(), Arg.Any<SqlParameter[]>()).Returns(_ => new List<Company> { new() { CompanyId = 1, CompanyName = "Acme Corp", CompanyAddress = "1 Main St" } });
        Data.QueryAsync<Department>(Arg.Any<string>(), Arg.Any<SqlParameter[]>()).Returns(_ => new List<Department> { new() { DepartmentId = 2, Name = "Engineering", CompanyId = 1 } });
        Data.QueryAsync<Position>(Arg.Any<string>(), Arg.Any<SqlParameter[]>()).Returns(_ => new List<Position> { new() { PositionId = 3, Name = "Developer", CompanyId = 1 } });
        Data.QueryAsync<Employee>(Arg.Any<string>(), Arg.Any<SqlParameter[]>()).Returns(_ => new List<Employee> { Sample() });
        Data.ExecuteNonQueryAsync(Arg.Any<string>(), Arg.Any<SqlParameter[]>()).Returns(1);
        Data.ScalarAsync(Arg.Any<string>(), Arg.Any<SqlParameter[]>()).Returns((object?)DBNull.Value);
    }

    public static Employee Sample() => new()
    {
        EmployeeId = 5, LastName = "Smith", FirstName = "John", MiddleName = "Paul", HireDate = new DateTime(2020, 3, 4),
        Salary = 1500m, DepartmentId = 2, PositionId = 3, CompanyId = 1,
    };

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(Environment);
        builder.ConfigureServices(services =>
        {
            var old = services.Where(d => d.ServiceType == typeof(IDataAccess)).ToList();
            foreach (var d in old)
            {
                services.Remove(d);
            }

            services.AddSingleton(Data);
        });
    }
}
