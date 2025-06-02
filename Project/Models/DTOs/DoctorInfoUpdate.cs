using System.ComponentModel.DataAnnotations;

namespace Models.DTOs;


public class DoctorInfoUpdate
{
    [Required, Range(0, double.MaxValue)]
    public decimal Salary { get; set; }

    [Required]
    public bool IsActive { get; set; }

    public int SpecializationId { get; set; }
}