
using System.ComponentModel.DataAnnotations;

namespace Models.DTOs;

public class MedicalRecordDto
{
    [MaxLength(200)]
    public string? VisitDescription { get; set; }

    [Required, MaxLength(200)]
    public string? Diagnosis { get; set; }

    [MaxLength(200)]
    public string? AdditionalNotes { get; set; }

    [Required]
    public int AppointmentId { get; set; }
    
}