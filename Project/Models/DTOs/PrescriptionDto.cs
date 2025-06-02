using System.ComponentModel.DataAnnotations;

namespace Models.DTOs;


public class PrescriptionDto
{
    [Required, MaxLength(50)]
    public string? MedicationName { get; set; }

    [Required, MaxLength(50)]
    public string? Dosage { get; set; }

    [Required, MaxLength(50)]
    public string? Frequency { get; set; }

    [Required]
    public DateOnly StartDate { get; set; }

    [Required]
    public DateOnly EndDate { get; set; }

    [MaxLength(200)]
    public string? SpecialInstructions { get; set; }

    public int MedicalRecordId { get; set; }
}