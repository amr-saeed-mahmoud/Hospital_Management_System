

using System.ComponentModel.DataAnnotations;

namespace Models.DTOs;

public class DepartmentDto
{
    [Required, MaxLength(50)]
    public string? Name { get; set; }

    [Required, MaxLength(150)]
    public string? Description { get; set; }

}