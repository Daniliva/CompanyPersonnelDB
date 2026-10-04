using System.Globalization;
using FluentAssertions;
using Project.Models;
using Project.Services;

namespace Project.Tests;

public class SalaryReportFormatterTests
{
    private static Employee E(int id = 1, string last = "Smith", string first = "John", string? middle = "Paul", decimal salary = 1000m, int dept = 2, int pos = 3) =>
        new() { EmployeeId = id, LastName = last, FirstName = first, MiddleName = middle, Salary = salary, DepartmentId = dept, PositionId = pos };

    [Fact]
    public void Empty_report_is_only_the_header_line()
    {
        SalaryReportFormatter.ToTxt(Array.Empty<Employee>())
            .Should().Be("Employee ID\tLast Name\tFirst Name\tMiddle Name\tSalary\tDepartment ID\tPosition ID\n");
    }

    [Fact]
    public void Each_employee_is_one_tab_separated_line()
    {
        var lines = SalaryReportFormatter.ToTxt(new[] { E(), E(2, "Ivanov", "Ivan", "Ivanovich", 2500.5m, 4, 5) }).Split('\n');

        lines[1].Should().Be("1\tSmith\tJohn\tPaul\t1000.00\t2\t3");
        lines[2].Should().Be("2\tIvanov\tIvan\tIvanovich\t2500.50\t4\t5");
        lines[3].Should().BeEmpty();
    }

    [Fact]
    public void A_missing_middle_name_is_an_empty_column()
    {
        SalaryReportFormatter.ToTxt(new[] { E(middle: null) }).Split('\n')[1].Should().Be("1\tSmith\tJohn\t\t1000.00\t2\t3");
    }

    [Fact]
    public void Salary_uses_the_invariant_culture_whatever_the_server_culture_is()
    {
        var old = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("de-DE");

            SalaryReportFormatter.ToTxt(new[] { E(salary: 1234.5m) }).Should().Contain("\t1234.50\t");
        }
        finally
        {
            CultureInfo.CurrentCulture = old;
        }
    }

    [Fact]
    public void Tabs_and_line_breaks_inside_a_name_cannot_break_the_table()
    {
        var text = SalaryReportFormatter.ToTxt(new[] { E(last: "Sm\tith", first: "Jo\r\nhn") });

        var row = text.Split('\n')[1];
        row.Split('\t').Should().HaveCount(7);
        row.Should().Contain("Sm ith").And.Contain("Jo  hn");
        text.Split('\n').Should().HaveCount(3);
    }

    [Fact]
    public void Lines_end_with_LF_not_the_platform_newline()
    {
        SalaryReportFormatter.ToTxt(new[] { E() }).Should().NotContain("\r");
    }

    [Fact]
    public void Header_constant_matches_the_first_line()
    {
        SalaryReportFormatter.ToTxt(Array.Empty<Employee>()).Should().StartWith(SalaryReportFormatter.Header);
    }
}
