using Microsoft.Data.SqlClient;
using Project.DataAccess;
using Project.Models;
using Project.Services.Interfaces;

namespace Project.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IDataAccess _dal;

    public EmployeeService(IDataAccess dal)
    {
        _dal = dal;
    }

    public async Task<List<Employee>> GetEmployeesAsync(int companyId, int? departmentId = null, int? positionId = null, string? lastName = null, string? firstName = null, string? middleName = null)
    {
        var query = EmployeeQueryBuilder.BuildEmployeeSearch(companyId, departmentId, positionId, lastName, firstName, middleName);
        return await _dal.QueryAsync<Employee>(query.Sql, query.Parameters.ToArray());
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int employeeId)
    {
        var results = await _dal.QueryAsync<Employee>(SqlQueries.GetEmployeeById, new SqlParameter("@employeeId", employeeId));
        return results.FirstOrDefault();
    }

    public async Task<bool> UpdateEmployeeAsync(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        var parameters = new[]
        {
            new SqlParameter("@employeeId", employee.EmployeeId),
            new SqlParameter("@lastName", employee.LastName.Trim()),
            new SqlParameter("@firstName", employee.FirstName.Trim()),
            new SqlParameter("@middleName", NullIfBlank(employee.MiddleName)),
            new SqlParameter("@address", NullIfBlank(employee.Address)),
            new SqlParameter("@phone", NullIfBlank(employee.Phone)),
            new SqlParameter("@birthDate", employee.BirthDate ?? (object)DBNull.Value),
            new SqlParameter("@hireDate", employee.HireDate),
            new SqlParameter("@salary", employee.Salary),
            new SqlParameter("@departmentId", employee.DepartmentId),
            new SqlParameter("@positionId", employee.PositionId)
        };

        return await _dal.ExecuteNonQueryAsync(SqlQueries.UpdateEmployee, parameters) > 0;
    }

    private static object NullIfBlank(string? value) =>
        string.IsNullOrWhiteSpace(value) ? DBNull.Value : value.Trim();
}
