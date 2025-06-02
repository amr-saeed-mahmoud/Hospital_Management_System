using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Models.Domain.Enums;
using Models.Domain.ValidationAttributes;

namespace Models.DTOs;


public class AppointmentDto
{
    [Required]
    [ScheduledDateTimeRange]
    public DateOnly ScheduledDateTime { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Required]
    public int DoctorId { get; set; }
    
    [Required]
    public int PatientId { get; set; }
    
}