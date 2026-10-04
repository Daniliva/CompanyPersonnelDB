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
        throw new NotImplementedException("TODO");
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int employeeId)
    {
        throw new NotImplementedException("TODO");
    }

    public async Task<bool> UpdateEmployeeAsync(Employee employee)
    {
        throw new NotImplementedException("TODO");
    }

    private static object NullIfBlank(string? value) =>
        throw new NotImplementedException("TODO");
}
