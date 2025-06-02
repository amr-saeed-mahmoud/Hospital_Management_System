using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Domain.Services;

public class Department
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string? Name { get; set; }

    [Required, MaxLength(150)]
    public string? Description { get; set; }

    [Required]
    public DateOnly CreatedAt { get; set; }

    [Required]
    public DateOnly UpdatedAt { get; set; }

    [NotMapped]
    public ICollection<Specialization>? Specializations { get; set; }

}