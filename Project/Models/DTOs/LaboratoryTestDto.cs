

using System.ComponentModel.DataAnnotations;

namespace Models.DTOs;

public class LaboratoryTestDto
{
    [Required, MaxLength(50)]
    public string? Name { get; set; }

    [Required, MaxLength(200)]
    public string? Result { get; set; }

    [Required, Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    public DateOnly ApplicationDate { get; set; }

    [Required]
    public DateOnly ReceiptDate { get; set; }

    public int PatientId { get; set; }
    public int CreatedById { get; set; }
}