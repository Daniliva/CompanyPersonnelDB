
using Microsoft.Data.SqlClient;
using Project.DataAccess;
using Project.Models;
using Project.Services.Interfaces;

namespace Project.Services;

public class SalaryService : ISalaryService
{
    private readonly DataAccessLayer _dal;

    public SalaryService(DataAccessLayer dal)
    {
        _dal = dal;
    }

    public async Task<List<Employee>> GetSalaryReportAsync(int companyId, int? departmentId = null, int? positionId = null)
    {
        var query = SqlQueries.GetSalaryReportBase;
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

        return await _dal.QueryAsync<Employee>(query, parameters.ToArray());
    }

    public async Task<decimal?> GetTotalSalaryByDepartmentAsync(int companyId, int departmentId, int? positionId)
    {

        var query = SqlQueries.GetTotalSalaryByDepartment;
        var parameters = new List<SqlParameter> { new SqlParameter("@departmentId", departmentId), new SqlParameter("@companyId", companyId),};

        if (positionId.HasValue)
        {
            query += SqlQueries.AppendPositionFilter;
            parameters.Add(new SqlParameter("@positionId", positionId.Value));
        }

        var result = await _dal.ScalarAsync(query,
            parameters.ToArray()
            );

        return result != DBNull.Value ? Convert.ToDecimal(result) : null;
    }
}