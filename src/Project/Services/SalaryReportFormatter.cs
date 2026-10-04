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
        throw new NotImplementedException("TODO");
    }

    private static string Clean(string? value) =>
        throw new NotImplementedException("TODO");
}
