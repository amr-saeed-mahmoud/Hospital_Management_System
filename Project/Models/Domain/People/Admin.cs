using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Domain.People;

[Table("Admins")]
public class Admin : Person
{
    
    [Required]
    public bool IsActive { get; set; }

    [Required, Range(0, double.MaxValue)]
    public decimal Salary { get; set; }
}