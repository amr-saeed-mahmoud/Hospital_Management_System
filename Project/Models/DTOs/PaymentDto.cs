using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Models.Domain.ValidationAttributes;

namespace Models.DTOs;

public class PaymentDto
{
    [Required]
    public int AppointmentId { get; set; }
    
    [Required, MaxLength(50)]
    public string? PaymentMethod { get; set; }

    [Required, Range(0, double.MaxValue)]
    public decimal AmountPaid { get; set; }

    [AllowNull]
    [MaxLength(200)]
    public string? AdditionalNotes { get; set; }
}