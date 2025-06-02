

using System.ComponentModel.DataAnnotations;

namespace Models.DTOs;


public class LabTechDto
{
    [Required]
    public bool IsActive { get; set; }

    [Required, Range(0, double.MaxValue)]
    public decimal Salary { get; set; }
}