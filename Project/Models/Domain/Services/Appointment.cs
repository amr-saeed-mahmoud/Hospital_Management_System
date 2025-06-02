
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Domain.Enums;
using Models.Domain.People;
using Models.Domain.ValidationAttributes;

namespace Models.Domain.Services;

[Table("Appointments")]
public class Appointment
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [ScheduledDateTimeRange]
    public DateOnly ScheduledDateTime { get; set; }

    [Required]
    public AppointmentStatus Status { get; set; }

    public int DoctorId { get; set; }
    [ForeignKey("DoctorId")]
    public Doctor? CurrentDoctor { get; set; }

    public int PatientId { get; set; }
    [ForeignKey("PatientId")]
    public Patient? CurrentPatient { get; set; }

}