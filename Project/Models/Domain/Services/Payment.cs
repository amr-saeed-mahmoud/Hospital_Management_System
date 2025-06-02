using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Domain.ValidationAttributes;

namespace Models.Domain.Services;

public class Payment
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ScheduledDateTimeRange]
    public DateOnly PaymentDate { get; set; }

    [Required, MaxLength(50)]
    public string? PaymentMethod { get; set; }

    [Required, Range(0, double.MaxValue)]
    public decimal AmountPaid { get; set; }

    [MaxLength(200)]
    public string? AdditionalNotes { get; set; }

    [Required]
    public int AppointmentId { get; set; }
    [ForeignKey("AppointmentId")]
    public Appointment? CurrentAppointment { get; set; }
}