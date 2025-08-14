using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models;

public class Employee
{
    [Column("employee_id")]
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
    [Column("last_name")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
    [Column("first_name")]
    public string FirstName { get; set; }

    [StringLength(100, ErrorMessage = "Middle name cannot exceed 100 characters")]
    [Column("middle_name")]
    public string MiddleName { get; set; }

    [StringLength(255, ErrorMessage = "Address cannot exceed 255 characters")]
    [Column("address")]
    public string Address { get; set; }

    [StringLength(50, ErrorMessage = "Phone cannot exceed 50 characters")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    [Column("phone")]
    public string Phone { get; set; }

    [Column("birth_date")]
    public DateTime? BirthDate { get; set; }

    [Required(ErrorMessage = "Hire date is required")]
    [Column("hire_date")]
    public DateTime HireDate { get; set; }

    [Required(ErrorMessage = "Salary is required")]
    [Range(0, 1000000, ErrorMessage = "Salary must be between 0 and 1,000,000")]
    [Column("salary")]
    public decimal Salary { get; set; }

    [Required(ErrorMessage = "Department ID is required")]
    [Column("department_id")]
    public int DepartmentId { get; set; }

    [Required(ErrorMessage = "Position ID is required")]
    [Column("position_id")]
    public int PositionId { get; set; }

    [Required(ErrorMessage = "Company ID is required")]
    [Column("company_id")]
    public int CompanyId { get; set; }
}