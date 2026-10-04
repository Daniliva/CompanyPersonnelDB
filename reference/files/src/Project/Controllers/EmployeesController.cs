using Project.Models;
using Project.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Services.Interfaces;
using System.Threading.Tasks;

namespace Project.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly ICompanyService _companyService;

        public EmployeesController(IEmployeeService employeeService, ICompanyService companyService)
        {
            _employeeService = employeeService;
            _companyService = companyService;
        }

        public async Task<IActionResult> Index(int? departmentId, int? positionId, string? lastName, string? firstName, string? middleName)
        {
            var employees = await _employeeService.GetEmployeesAsync(1, departmentId, positionId, lastName, firstName, middleName);

            ViewBag.Departments = await _companyService.GetDepartmentsAsync(1);
            ViewBag.Positions = await _companyService.GetPositionsAsync(1);

            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound();

            ViewBag.Departments = await _companyService.GetDepartmentsAsync(1);
            ViewBag.Positions = await _companyService.GetPositionsAsync(1);

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var updated = await _employeeService.UpdateEmployeeAsync(employee);
                if (!updated) return NotFound();
                return RedirectToAction("Index");
            }

            ViewBag.Departments = await _companyService.GetDepartmentsAsync(1);
            ViewBag.Positions = await _companyService.GetPositionsAsync(1);

            return View(employee);
        }
    }
}