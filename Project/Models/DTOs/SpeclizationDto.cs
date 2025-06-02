

using System.ComponentModel.DataAnnotations;

namespace Models.DTOs;

public class SpeclizationDto
{
    [Required, MaxLength(50)]
    public string? Name { get; set; }

    [Required, MaxLength(150)]
    public string? Description { get; set; }

    public int DepartmentId { get; set; }
}