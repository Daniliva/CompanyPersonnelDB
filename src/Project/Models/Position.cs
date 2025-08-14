using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models;

public class Position
{
    [Column("position_id")]
    public int PositionId { get; set; }

    [Required(ErrorMessage = "Position name is required")]
    [StringLength(100, ErrorMessage = "Position name cannot exceed 100 characters")]
    [Column("name")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Company ID is required")]
    [Column("company_id")]
    public int CompanyId { get; set; }
}