
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Domain.People;


[Table("LabTechs")]
public class LabTech : Person
{

    [Required]
    public bool IsActive { get; set; }

    [Required, Range(0, double.MaxValue)]
    public decimal Salary { get; set; }
}