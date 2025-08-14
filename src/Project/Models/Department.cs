using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models;

public class Department
{
    [Column("department_id")]
    public int DepartmentId { get; set; }

    [Required(ErrorMessage = "Department name is required")]
    [StringLength(100, ErrorMessage = "Department name cannot exceed 100 characters")]
    [Column("name")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Company ID is required")]
    [Column("company_id")]
    public int CompanyId { get; set; }
}