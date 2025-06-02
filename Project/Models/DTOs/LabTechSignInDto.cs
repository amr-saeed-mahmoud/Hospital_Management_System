
using System.ComponentModel.DataAnnotations;

namespace Models.DTOs;


public class LabTechSignInDto : PersonDto
{
    [Required, Range(1, double.MaxValue)]
    public decimal Salary { get; set; }

    [Required]
    public bool IsActive { get; set; }
}