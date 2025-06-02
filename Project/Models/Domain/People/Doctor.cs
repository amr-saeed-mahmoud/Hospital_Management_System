using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Domain.Services;

namespace Models.Domain.People;

[Table("Doctors")]
public class Doctor : Person
{

    [Required, Range(0, double.MaxValue)]
    public decimal Salary { get; set; }

    [Required]
    public bool IsActive { get; set; }

    public int SpecializationId { get; set; }
    [ForeignKey("SpecializationId")]
    public Specialization? CurrentSpecialization { get; set; }
}