using Microsoft.Data.SqlClient;
using Project.DataAccess;
using Project.Models;
using Project.Services.Interfaces;

namespace Project.Services;

public class EmployeeService : IEmployeeService
{
    private readonly DataAccessLayer _dal;

    public EmployeeService(DataAccessLayer dal)
    {
        _dal = dal;
    }

    public async Task<List<Employee>> GetEmployeesAsync(int companyId, int? departmentId = null, int? positionId = null, string lastName = null, string firstName = null, string middleName = null)
    {
        var query = SqlQueries.GetEmployeesBase;
        var parameters = new List<SqlParameter> { new SqlParameter("@companyId", companyId) };

        if (departmentId.HasValue)
        {
            query += SqlQueries.AppendDepartmentFilter;
            parameters.Add(new SqlParameter("@departmentId", departmentId.Value));
        }

        if (positionId.HasValue)
        {
            query += SqlQueries.AppendPositionFilter;
            parameters.Add(new SqlParameter("@positionId", positionId.Value));
        }

        if (!string.IsNullOrEmpty(lastName) || !string.IsNullOrEmpty(firstName) || !string.IsNullOrEmpty(middleName))
        {
            query += SqlQueries.AppendNameFilter;
            parameters.Add(new SqlParameter("@lastName", lastName?.Trim() ?? ""));
            parameters.Add(new SqlParameter("@firstName", firstName?.Trim() ?? ""));
            parameters.Add(new SqlParameter("@middleName", middleName?.Trim() ?? ""));
        }

        return await _dal.QueryAsync<Employee>(query, parameters.ToArray());
    }

    public async Task<Employee> GetEmployeeByIdAsync(int employeeId)
    {
        var results = await _dal.QueryAsync<Employee>(SqlQueries.GetEmployeeById, new SqlParameter("@employeeId", employeeId));
        return results.FirstOrDefault();
    }

    public async Task UpdateEmployeeAsync(Employee employee)
    {
        var parameters = new[]
        {
            new SqlParameter("@employeeId", employee.EmployeeId),
            new SqlParameter("@lastName", employee.LastName.Trim()),
            new SqlParameter("@firstName", employee.FirstName.Trim()),
            new SqlParameter("@middleName", employee.MiddleName.Trim() ?? (object)DBNull.Value),
            new SqlParameter("@address", employee.Address ?? (object)DBNull.Value),
            new SqlParameter("@phone", employee.Phone ?? (object)DBNull.Value),
            new SqlParameter("@birthDate", employee.BirthDate ?? (object)DBNull.Value),
            new SqlParameter("@hireDate", employee.HireDate),
            new SqlParameter("@salary", employee.Salary),
            new SqlParameter("@departmentId", employee.DepartmentId),
            new SqlParameter("@positionId", employee.PositionId)
        };

        await _dal.ExecuteNonQueryAsync(SqlQueries.UpdateEmployee, parameters);
    }
}