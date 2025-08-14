using Project.Models;

namespace Project.Services.Interfaces;

public interface ISalaryService
{
    Task<List<Employee>> GetSalaryReportAsync(int companyId, int? departmentId = null, int? positionId = null);
    Task<decimal?> GetTotalSalaryByDepartmentAsync(int companyId, int departmentId, int? positionId);
}