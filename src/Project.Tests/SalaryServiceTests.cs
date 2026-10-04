using FluentAssertions;
using Microsoft.Data.SqlClient;
using NSubstitute;
using Project.DataAccess;
using Project.Models;
using Project.Services;

namespace Project.Tests;

public class SalaryServiceTests
{
    private readonly IDataAccess dal = Substitute.For<IDataAccess>();
    private readonly SalaryService service;

    public SalaryServiceTests() => service = new SalaryService(dal);

    [Fact]
    public async Task Report_without_filters_is_the_base_query_ordered()
    {
        dal.QueryAsync<Employee>(Arg.Any<string>(), Arg.Any<SqlParameter[]>()).Returns(new List<Employee>());

        await service.GetSalaryReportAsync(1);

        await dal.Received().QueryAsync<Employee>(
            SqlQueries.GetSalaryReportBase + " ORDER BY last_name, first_name, employee_id",
            Arg.Is<SqlParameter[]>(ps => ps.Length == 1 && ps[0].ParameterName == "@companyId"));
    }

    [Fact]
    public async Task Report_adds_department_and_position_filters_before_the_ordering()
    {
        dal.QueryAsync<Employee>(Arg.Any<string>(), Arg.Any<SqlParameter[]>()).Returns(new List<Employee>());

        await service.GetSalaryReportAsync(1, 2, 3);

        await dal.Received().QueryAsync<Employee>(
            Arg.Is<string>(s => s.Contains("department_id = @departmentId") && s.Contains("position_id = @positionId") && s.EndsWith("employee_id")),
            Arg.Is<SqlParameter[]>(ps => ps.Length == 3));
    }

    [Fact]
    public async Task Total_is_null_when_the_database_returns_SQL_NULL()
    {
        dal.ScalarAsync(Arg.Any<string>(), Arg.Any<SqlParameter[]>()).Returns(DBNull.Value);

        (await service.GetTotalSalaryByDepartmentAsync(1, 2, null)).Should().BeNull();
    }

    [Fact]
    public async Task Total_is_null_when_there_is_no_result_at_all()
    {
        dal.ScalarAsync(Arg.Any<string>(), Arg.Any<SqlParameter[]>()).Returns((object?)null);

        (await service.GetTotalSalaryByDepartmentAsync(1, 2, null)).Should().BeNull();
    }

    [Fact]
    public async Task Total_is_the_decimal_value()
    {
        dal.ScalarAsync(Arg.Any<string>(), Arg.Any<SqlParameter[]>()).Returns(12345.67m);

        (await service.GetTotalSalaryByDepartmentAsync(1, 2, null)).Should().Be(12345.67m);
    }

    [Fact]
    public async Task Total_without_position_uses_two_parameters()
    {
        dal.ScalarAsync(Arg.Any<string>(), Arg.Any<SqlParameter[]>()).Returns(1m);

        await service.GetTotalSalaryByDepartmentAsync(1, 2, null);

        await dal.Received().ScalarAsync(SqlQueries.GetTotalSalaryByDepartment,
            Arg.Is<SqlParameter[]>(ps => ps.Length == 2 && ps.Any(p => p.ParameterName == "@departmentId" && (int)p.Value == 2)));
    }

    [Fact]
    public async Task Total_with_position_adds_the_position_filter()
    {
        dal.ScalarAsync(Arg.Any<string>(), Arg.Any<SqlParameter[]>()).Returns(1m);

        await service.GetTotalSalaryByDepartmentAsync(1, 2, 9);

        await dal.Received().ScalarAsync(
            SqlQueries.GetTotalSalaryByDepartment + SqlQueries.AppendPositionFilter,
            Arg.Is<SqlParameter[]>(ps => ps.Length == 3 && ps.Any(p => p.ParameterName == "@positionId" && (int)p.Value == 9)));
    }
}
