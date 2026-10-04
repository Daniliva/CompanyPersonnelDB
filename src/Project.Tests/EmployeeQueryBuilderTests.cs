using FluentAssertions;
using Project.DataAccess;

namespace Project.Tests;

public class EmployeeQueryBuilderTests
{
    private static BuiltQuery Build(int? dept = null, int? pos = null, string? last = null, string? first = null, string? middle = null) =>
        EmployeeQueryBuilder.BuildEmployeeSearch(1, dept, pos, last, first, middle);

    private static object? Value(BuiltQuery q, string name) => q.Parameters.Single(p => p.ParameterName == name).Value;

    [Fact]
    public void Without_filters_it_is_the_base_query_ordered_deterministically()
    {
        var q = Build();

        q.Sql.Should().Be(SqlQueries.GetEmployeesBase + " ORDER BY last_name, first_name, employee_id");
        q.Parameters.Should().ContainSingle().Which.ParameterName.Should().Be("@companyId");
        Value(q, "@companyId").Should().Be(1);
    }

    [Fact]
    public void Company_id_is_taken_from_the_argument()
    {
        var q = EmployeeQueryBuilder.BuildEmployeeSearch(42, null, null, null, null, null);

        Value(q, "@companyId").Should().Be(42);
    }

    [Fact]
    public void Department_filter_is_added_with_a_parameter()
    {
        var q = Build(dept: 3);

        q.Sql.Should().Contain("AND department_id = @departmentId");
        Value(q, "@departmentId").Should().Be(3);
    }

    [Fact]
    public void Position_filter_is_added_with_a_parameter()
    {
        var q = Build(pos: 7);

        q.Sql.Should().Contain("AND position_id = @positionId");
        Value(q, "@positionId").Should().Be(7);
    }

    [Fact]
    public void Without_those_filters_the_parameters_are_not_declared()
    {
        var q = Build(last: "Smith");

        q.Parameters.Select(p => p.ParameterName).Should().NotContain(new[] { "@departmentId", "@positionId", "@firstName", "@middleName" });
    }

    [Fact]
    public void Department_and_position_can_be_combined()
    {
        var q = Build(dept: 1, pos: 2);

        q.Sql.Should().Contain("department_id = @departmentId").And.Contain("position_id = @positionId");
        q.Parameters.Should().HaveCount(3);
    }

    [Fact]
    public void Last_name_adds_only_the_last_name_condition()
    {
        var q = Build(last: "Smi");

        q.Sql.Should().Contain("AND last_name LIKE '%' + @lastName + '%' ESCAPE '\\'");
        q.Sql.Should().NotContain("first_name LIKE").And.NotContain("middle_name LIKE");
    }

    [Fact]
    public void An_employee_without_a_middle_name_is_not_excluded_by_a_last_name_search()
    {
        // The old query always ANDed "middle_name LIKE '%%'", which is UNKNOWN for NULL and dropped these employees.
        Build(last: "Smith").Sql.Should().NotContain("middle_name LIKE");
    }

    [Fact]
    public void Each_provided_part_adds_its_own_condition_combined_with_AND()
    {
        var q = Build(last: "Smith", first: "John", middle: "Paul");

        q.Sql.Should().Contain("AND last_name LIKE").And.Contain("AND first_name LIKE").And.Contain("AND middle_name LIKE");
        q.Sql.Should().NotContain(" OR ");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void A_blank_part_adds_no_condition(string? blank)
    {
        var q = Build(last: blank, first: blank, middle: blank);

        q.Sql.Should().NotContain("LIKE");
        q.Parameters.Should().ContainSingle();
    }

    [Fact]
    public void Name_parts_are_trimmed()
    {
        Value(Build(last: "  Smith  "), "@lastName").Should().Be("Smith");
    }

    [Theory]
    [InlineData("50%", "50\\%")]
    [InlineData("a_b", "a\\_b")]
    [InlineData("[abc]", "\\[abc]")]
    [InlineData("back\\slash", "back\\\\slash")]
    [InlineData("O'Brien", "O'Brien")]
    public void LIKE_wildcards_in_the_value_are_escaped_so_they_match_literally(string input, string expected)
    {
        Value(Build(last: input), "@lastName").Should().Be(expected);
    }

    [Fact]
    public void User_input_never_appears_in_the_sql_text()
    {
        const string evil = "x'; DROP TABLE employees; --";

        var q = Build(last: evil, first: evil, middle: evil);

        q.Sql.Should().NotContain("DROP").And.NotContain("--");
        Value(q, "@lastName").Should().Be(evil);
    }

    [Fact]
    public void Order_by_is_the_last_clause()
    {
        var q = Build(dept: 1, pos: 1, last: "a", first: "b", middle: "c");

        q.Sql.Should().EndWith(" ORDER BY last_name, first_name, employee_id");
        q.Sql.Split("ORDER BY").Length.Should().Be(2);
    }

    [Fact]
    public void EscapeLike_leaves_ordinary_text_alone()
    {
        EmployeeQueryBuilder.EscapeLike("Иванов").Should().Be("Иванов");
    }
}
