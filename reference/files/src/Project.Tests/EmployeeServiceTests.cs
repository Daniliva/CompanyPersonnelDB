using FluentAssertions;
using Microsoft.Data.SqlClient;
using NSubstitute;
using Project.DataAccess;
using Project.Models;
using Project.Services;

namespace Project.Tests;

public class EmployeeServiceTests
{
    private readonly IDataAccess dal = Substitute.For<IDataAccess>();
    private readonly EmployeeService service;

    public EmployeeServiceTests()
    {
        service = new EmployeeService(dal);
        dal.ExecuteNonQueryAsync(Arg.Any<string>(), Arg.Any<SqlParameter[]>()).Returns(1);
    }

    private static Employee Sample() => new()
    {
        EmployeeId = 5, LastName = "  Smith ", FirstName = " John ", MiddleName = " Paul ", Address = " Main st ", Phone = " +1 555 ",
        BirthDate = new DateTime(1990, 1, 2), HireDate = new DateTime(2020, 3, 4), Salary = 1234.5m, DepartmentId = 2, PositionId = 3, CompanyId = 1,
    };

    private SqlParameter[] LastUpdateParameters() =>
        (SqlParameter[])dal.ReceivedCalls().Last(c => c.GetMethodInfo().Name == nameof(IDataAccess.ExecuteNonQueryAsync)).GetArguments()[1]!;

    private static object? P(SqlParameter[] ps, string name) => ps.Single(p => p.ParameterName == name).Value;

    [Fact]
    public async Task GetEmployees_runs_the_query_built_by_the_builder()
    {
        dal.QueryAsync<Employee>(Arg.Any<string>(), Arg.Any<SqlParameter[]>()).Returns(new List<Employee> { new() { EmployeeId = 1 } });

        var result = await service.GetEmployeesAsync(1, 2, null, "Smi", null, null);

        result.Should().ContainSingle();
        await dal.Received(1).QueryAsync<Employee>(
            Arg.Is<string>(s => s.Contains("department_id = @departmentId") && s.Contains("last_name LIKE") && !s.Contains("middle_name LIKE")),
            Arg.Is<SqlParameter[]>(ps => ps.Any(p => p.ParameterName == "@lastName" && (string)p.Value == "Smi")));
    }

    [Fact]
    public async Task GetEmployeeById_returns_null_when_there_is_no_row()
    {
        dal.QueryAsync<Employee>(Arg.Any<string>(), Arg.Any<SqlParameter[]>()).Returns(new List<Employee>());

        (await service.GetEmployeeByIdAsync(99)).Should().BeNull();
    }

    [Fact]
    public async Task GetEmployeeById_returns_the_first_row_and_passes_the_id()
    {
        dal.QueryAsync<Employee>(SqlQueries.GetEmployeeById, Arg.Any<SqlParameter[]>()).Returns(new List<Employee> { new() { EmployeeId = 7 } });

        var e = await service.GetEmployeeByIdAsync(7);

        e!.EmployeeId.Should().Be(7);
        await dal.Received().QueryAsync<Employee>(SqlQueries.GetEmployeeById, Arg.Is<SqlParameter[]>(ps => (int)ps.Single().Value == 7));
    }

    [Fact]
    public async Task Update_without_a_middle_name_does_not_crash_and_stores_NULL()
    {
        var e = Sample();
        e.MiddleName = null;

        var updated = await service.UpdateEmployeeAsync(e);

        updated.Should().BeTrue();
        P(LastUpdateParameters(), "@middleName").Should().Be(DBNull.Value);
    }

    [Fact]
    public async Task Update_with_a_blank_middle_name_stores_NULL()
    {
        var e = Sample();
        e.MiddleName = "   ";

        await service.UpdateEmployeeAsync(e);

        P(LastUpdateParameters(), "@middleName").Should().Be(DBNull.Value);
    }

    [Fact]
    public async Task Update_trims_the_text_fields()
    {
        await service.UpdateEmployeeAsync(Sample());

        var ps = LastUpdateParameters();
        P(ps, "@lastName").Should().Be("Smith");
        P(ps, "@firstName").Should().Be("John");
        P(ps, "@middleName").Should().Be("Paul");
        P(ps, "@address").Should().Be("Main st");
        P(ps, "@phone").Should().Be("+1 555");
    }

    [Fact]
    public async Task Update_stores_blank_address_and_phone_as_NULL()
    {
        var e = Sample();
        e.Address = "";
        e.Phone = null;

        await service.UpdateEmployeeAsync(e);

        P(LastUpdateParameters(), "@address").Should().Be(DBNull.Value);
        P(LastUpdateParameters(), "@phone").Should().Be(DBNull.Value);
    }

    [Fact]
    public async Task Update_passes_every_column_of_the_UPDATE_statement()
    {
        await service.UpdateEmployeeAsync(Sample());

        var ps = LastUpdateParameters();
        ps.Select(p => p.ParameterName).Should().BeEquivalentTo(
            "@employeeId", "@lastName", "@firstName", "@middleName", "@address", "@phone", "@birthDate", "@hireDate", "@salary", "@departmentId", "@positionId");
        P(ps, "@employeeId").Should().Be(5);
        P(ps, "@salary").Should().Be(1234.5m);
        P(ps, "@departmentId").Should().Be(2);
        P(ps, "@positionId").Should().Be(3);
        P(ps, "@birthDate").Should().Be(new DateTime(1990, 1, 2));
        P(ps, "@hireDate").Should().Be(new DateTime(2020, 3, 4));
    }

    [Fact]
    public async Task Update_with_no_birth_date_stores_NULL()
    {
        var e = Sample();
        e.BirthDate = null;

        await service.UpdateEmployeeAsync(e);

        P(LastUpdateParameters(), "@birthDate").Should().Be(DBNull.Value);
    }

    [Fact]
    public async Task Update_returns_false_when_no_row_was_changed()
    {
        dal.ExecuteNonQueryAsync(Arg.Any<string>(), Arg.Any<SqlParameter[]>()).Returns(0);

        (await service.UpdateEmployeeAsync(Sample())).Should().BeFalse();
    }

    [Fact]
    public async Task Update_of_null_throws()
    {
        Func<Task> act = () => service.UpdateEmployeeAsync(null!);

        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
