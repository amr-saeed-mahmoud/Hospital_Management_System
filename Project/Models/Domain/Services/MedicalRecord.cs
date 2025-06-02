using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Domain.Services;

public class MedicalRecord
{
    [Key]
    public int Id { get; set; }

    [MaxLength(200)]
    public string? VisitDescription { get; set; }

    [Required, MaxLength(200)]
    public string? Diagnosis { get; set; }

    [MaxLength(200)]
    public string? AdditionalNotes { get; set; }

    [NotMapped]
    public ICollection<Prescription>? Prescriptions { get; set; }

    [Required]
    public int AppointmentId { get; set; }
    [ForeignKey("AppointmentId")]
    public Appointment? CurrentAppointment { get; set; }

}