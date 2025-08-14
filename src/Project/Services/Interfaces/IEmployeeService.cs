using Project.Models;

namespace Project.Services.Interfaces;

public interface IEmployeeService
{
    Task<List<Employee>> GetEmployeesAsync(int companyId, int? departmentId = null, int? positionId = null, string lastName = null, string firstName = null, string middleName = null);
    Task<Employee> GetEmployeeByIdAsync(int employeeId);
    Task UpdateEmployeeAsync(Employee employee);
}