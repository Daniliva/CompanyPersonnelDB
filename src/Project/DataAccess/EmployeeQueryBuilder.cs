using Microsoft.Data.SqlClient;

namespace Project.DataAccess;

/// <summary>A SQL text together with its parameters. Values NEVER appear inside the text.</summary>
public sealed record BuiltQuery(string Sql, IReadOnlyList<SqlParameter> Parameters);

/// <summary>Builds the employee list query from the filters of the page.</summary>
public static class EmployeeQueryBuilder
{
    /// <summary>
    /// Builds: <see cref="SqlQueries.GetEmployeesBase"/> + optional filters + <c>ORDER BY last_name, first_name, employee_id</c>.
    /// <list type="bullet">
    /// <item>department/position filters add <see cref="SqlQueries.AppendDepartmentFilter"/> / <see cref="SqlQueries.AppendPositionFilter"/> with @departmentId / @positionId;</item>
    /// <item>each name part is trimmed; a part that is null, empty or whitespace adds NO condition (so an employee without a middle name is still found by last name);</item>
    /// <item>each non-empty part adds <c>AND last_name LIKE '%' + @lastName + '%' ESCAPE '\'</c> (likewise first_name / middle_name) — the parts combine with AND;</item>
    /// <item>the characters <c>\ % _ [</c> in the value are escaped with a backslash, so "50%" is searched literally;</item>
    /// <item>@companyId is always the first parameter; the SQL text never contains user input.</item>
    /// </list>
    /// </summary>
    public static BuiltQuery BuildEmployeeSearch(int companyId, int? departmentId, int? positionId, string? lastName, string? firstName, string? middleName)
    {
        throw new NotImplementedException("TODO");
    }

    /// <summary>Escapes <c>\ % _ [</c> with a backslash for use with <c>LIKE ... ESCAPE '\'</c>.</summary>
    public static string EscapeLike(string value) =>
        throw new NotImplementedException("TODO");

    private static void AddNamePart(ref string sql, List<SqlParameter> parameters, string column, string parameter, string? value)
    {
        throw new NotImplementedException("TODO");
    }
}
