using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models;

public class Company
{
    [Column("company_id")]
    public int CompanyId { get; set; }

    [Required(ErrorMessage = "Company name is required")]
    [StringLength(255, ErrorMessage = "Company name cannot exceed 255 characters")]
    [Column("company_name")]
    public string CompanyName { get; set; }

    [StringLength(255, ErrorMessage = "Company address cannot exceed 255 characters")]
    [Column("company_address")]
    public string CompanyAddress { get; set; }

    [StringLength(50, ErrorMessage = "Company phone cannot exceed 50 characters")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    [Column("company_phone")]
    public string CompanyPhone { get; set; }

    [Column("description")]
    public string Description { get; set; }
}