using System.ComponentModel.DataAnnotations;

namespace Models.DTOs;

public class DoctorSignInDto : PersonDto
{
    [Required]
    public bool IsActive { get; set; }
    [Required, Range(1, double.MaxValue)]
    public decimal Salary { get; set; }
    [Required]
    public int SpecializationId { get; set; }
}