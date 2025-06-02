using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Antiforgery;

namespace Models.DTOs;

public class NurseSIgnInDto : PersonDto
{
    [Required]
    public bool IsActive { get; set; }
    [Required, Range(1, double.MaxValue)]
    public decimal Salary { get; set; }
    [Required]
    public int DepartmentId { get; set; }
}