using Project.Models;

namespace Project.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<Company> GetCompanyAsync(int companyId);
        Task<List<Department>> GetDepartmentsAsync(int companyId);
        Task<List<Position>> GetPositionsAsync(int companyId);
    }
}