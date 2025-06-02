using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Domain.Services;

public class Prescription
{
    [Key]
    public int Id { get; set; }

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
    [ForeignKey("MedicalRecordId")]
    public MedicalRecord? CurrentMedicalRecord { get; set; }
    
}