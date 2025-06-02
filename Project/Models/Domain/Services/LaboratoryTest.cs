using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Domain.People;

namespace Models.Domain.Services;

public class LaboratoryTest
{
    [Key]
    public int Id { get; set; }

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
    [ForeignKey("PatientId")]
    public Patient? CurrentPatient { get; set; }

    public int CreatedById { get; set; }
    [ForeignKey("CreatedById")]
    public LabTech? CreatedBy { get; set; }

}