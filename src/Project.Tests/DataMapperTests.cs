using System.Data;
using FluentAssertions;
using Project.DataAccess;
using Project.Models;

namespace Project.Tests;

public class DataMapperTests
{
    private static IDataRecord Row(Action<DataTable> build)
    {
        var table = new DataTable();
        build(table);
        var reader = table.CreateDataReader();
        reader.Read();
        return reader;
    }

    [Fact]
    public void Columns_are_mapped_by_the_Column_attribute()
    {
        var row = Row(t =>
        {
            t.Columns.Add("employee_id", typeof(int));
            t.Columns.Add("last_name", typeof(string));
            t.Columns.Add("salary", typeof(decimal));
            t.Rows.Add(5, "Smith", 1500.25m);
        });

        var e = new DataMapper<Employee>().Map(row);

        e.EmployeeId.Should().Be(5);
        e.LastName.Should().Be("Smith");
        e.Salary.Should().Be(1500.25m);
    }

    [Fact]
    public void Column_names_are_case_insensitive()
    {
        var row = Row(t =>
        {
            t.Columns.Add("LAST_NAME", typeof(string));
            t.Rows.Add("Smith");
        });

        new DataMapper<Employee>().Map(row).LastName.Should().Be("Smith");
    }

    [Fact]
    public void Columns_without_the_attribute_are_matched_by_property_name()
    {
        var row = Row(t =>
        {
            t.Columns.Add("RequestId", typeof(string));
            t.Rows.Add("abc");
        });

        new DataMapper<ErrorViewModel>().Map(row).RequestId.Should().Be("abc");
    }

    [Fact]
    public void SQL_NULL_leaves_the_default_value()
    {
        var row = Row(t =>
        {
            t.Columns.Add("middle_name", typeof(string));
            t.Columns.Add("birth_date", typeof(DateTime));
            t.Rows.Add(DBNull.Value, DBNull.Value);
        });

        var e = new DataMapper<Employee>().Map(row);

        e.MiddleName.Should().BeNull();
        e.BirthDate.Should().BeNull();
    }

    [Fact]
    public void A_nullable_property_receives_the_value()
    {
        var row = Row(t =>
        {
            t.Columns.Add("birth_date", typeof(DateTime));
            t.Rows.Add(new DateTime(1990, 5, 6));
        });

        new DataMapper<Employee>().Map(row).BirthDate.Should().Be(new DateTime(1990, 5, 6));
    }

    [Fact]
    public void Values_are_converted_to_the_property_type()
    {
        var row = Row(t =>
        {
            t.Columns.Add("employee_id", typeof(long));
            t.Columns.Add("salary", typeof(double));
            t.Rows.Add(7L, 10.5d);
        });

        var e = new DataMapper<Employee>().Map(row);

        e.EmployeeId.Should().Be(7);
        e.Salary.Should().Be(10.5m);
    }

    [Fact]
    public void Unknown_columns_are_ignored()
    {
        var row = Row(t =>
        {
            t.Columns.Add("no_such_column", typeof(string));
            t.Columns.Add("first_name", typeof(string));
            t.Rows.Add("zzz", "John");
        });

        new DataMapper<Employee>().Map(row).FirstName.Should().Be("John");
    }

    [Fact]
    public void A_value_that_cannot_be_converted_does_not_stop_the_other_columns()
    {
        var row = Row(t =>
        {
            t.Columns.Add("employee_id", typeof(string));
            t.Columns.Add("first_name", typeof(string));
            t.Rows.Add("not a number", "John");
        });

        var e = new DataMapper<Employee>().Map(row);

        e.EmployeeId.Should().Be(0);
        e.FirstName.Should().Be("John");
    }

    [Fact]
    public void Company_columns_map_to_the_model()
    {
        var row = Row(t =>
        {
            t.Columns.Add("company_id", typeof(int));
            t.Columns.Add("company_name", typeof(string));
            t.Rows.Add(1, "Acme");
        });

        var c = new DataMapper<Company>().Map(row);

        c.CompanyId.Should().Be(1);
        c.CompanyName.Should().Be("Acme");
    }
}
