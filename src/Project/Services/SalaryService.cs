using Microsoft.Data.SqlClient;
using Project.DataAccess;
using Project.Models;
using Project.Services.Interfaces;

namespace Project.Services;

public class SalaryService : ISalaryService
{
    private readonly IDataAccess _dal;

    public SalaryService(IDataAccess dal)
    {
        _dal = dal;
    }

    public async Task<List<Employee>> GetSalaryReportAsync(int companyId, int? departmentId = null, int? positionId = null)
    {
        throw new NotImplementedException("TODO");
    }

    public async Task<decimal?> GetTotalSalaryByDepartmentAsync(int companyId, int departmentId, int? positionId)
    {
        throw new NotImplementedException("TODO");
    }
}
