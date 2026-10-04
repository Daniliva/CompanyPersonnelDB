using System.Globalization;
using System.Text;
using Project.Models;

namespace Project.Services;

/// <summary>Produces the tab-separated text of the salary report.</summary>
public static class SalaryReportFormatter
{
    public const string Header = "Employee ID\tLast Name\tFirst Name\tMiddle Name\tSalary\tDepartment ID\tPosition ID";

    /// <summary>
    /// The header line, then one line per employee, every line ending with "\n" (not Environment.NewLine).
    /// Columns are separated by one tab. Salary uses the invariant culture with two decimals ("1234.50") whatever the server culture is.
    /// A null middle name is an empty column. Tabs, CR and LF inside a name are replaced by a single space so a name can never break the table.
    /// </summary>
    public static string ToTxt(IEnumerable<Employee> employees)
    {
        var sb = new StringBuilder();
        sb.Append(Header).Append('\n');

        foreach (var e in employees)
        {
            sb.Append(e.EmployeeId.ToString(CultureInfo.InvariantCulture)).Append('\t')
              .Append(Clean(e.LastName)).Append('\t')
              .Append(Clean(e.FirstName)).Append('\t')
              .Append(Clean(e.MiddleName)).Append('\t')
              .Append(e.Salary.ToString("F2", CultureInfo.InvariantCulture)).Append('\t')
              .Append(e.DepartmentId.ToString(CultureInfo.InvariantCulture)).Append('\t')
              .Append(e.PositionId.ToString(CultureInfo.InvariantCulture)).Append('\n');
        }

        return sb.ToString();
    }

    private static string Clean(string? value) =>
        value is null ? string.Empty : value.Replace('\t', ' ').Replace('\r', ' ').Replace('\n', ' ');
}
