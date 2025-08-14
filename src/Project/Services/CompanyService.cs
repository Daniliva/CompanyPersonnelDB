using Microsoft.Data.SqlClient;
using Project.DataAccess;
using Project.Models;
using Project.Services.Interfaces;

namespace Project.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly DataAccessLayer _dal;

        public CompanyService(DataAccessLayer dal)
        {
            _dal = dal;
        }

        public async Task<Company> GetCompanyAsync(int companyId)
        {
            var results = await _dal.QueryAsync<Company>(SqlQueries.GetCompanyById,  new SqlParameter("@companyId", companyId));
            return results.FirstOrDefault()!;
        }

        public async Task<List<Department>> GetDepartmentsAsync(int companyId)
        {
            return await _dal.QueryAsync<Department>(SqlQueries.GetAllDepartments,  new SqlParameter("@companyId", companyId));
        }

        public async Task<List<Position>> GetPositionsAsync(int companyId)
        {
            return await _dal.QueryAsync<Position>(SqlQueries.GetAllPositions,  new SqlParameter("@companyId", companyId));
        }
    }
}
