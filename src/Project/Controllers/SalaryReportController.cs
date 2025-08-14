using System.Text;
using Microsoft.AspNetCore.Mvc;
using Project.Services.Interfaces;

namespace Project.Controllers;

public class SalaryReportController : Controller
{
    private readonly ISalaryService _salaryService;
    private readonly ICompanyService _companyService;

    public SalaryReportController(ISalaryService salaryService, ICompanyService companyService)
    {
        _salaryService = salaryService;
        _companyService = companyService;
    }

    public async Task<IActionResult> Index(int? departmentId, int? positionId)
    {
        decimal? totalSalary = null;
        if (departmentId.HasValue)
        {
            totalSalary = await _salaryService.GetTotalSalaryByDepartmentAsync(1, departmentId.Value, positionId);
        }

        var employees = await _salaryService.GetSalaryReportAsync(1, departmentId, positionId);

        ViewBag.TotalSalary = totalSalary;
        ViewBag.Departments = await _companyService.GetDepartmentsAsync(1);
        ViewBag.Positions = await _companyService.GetPositionsAsync(1);
        ViewBag.Employees = employees;

        return View();
    }

    public async Task<IActionResult> ExportToTxt(int? departmentId, int? positionId)
    {
        var employees = await _salaryService.GetSalaryReportAsync(1, departmentId, positionId);

        var sb = new StringBuilder();
        sb.AppendLine("Employee ID\tLast Name\tFirst Name\tMiddle Name\tSalary\tDepartment ID\tPosition ID");

        foreach (var emp in employees)
        {
            sb.AppendLine($"{emp.EmployeeId}\t{emp.LastName}\t{emp.FirstName}\t{emp.MiddleName}\t{emp.Salary}\t{emp.DepartmentId}\t{emp.PositionId}");
        }

        return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/plain", "salary_report.txt");
    }
}