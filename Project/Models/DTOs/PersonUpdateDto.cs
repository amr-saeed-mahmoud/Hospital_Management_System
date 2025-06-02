using System.ComponentModel.DataAnnotations;

namespace Models.DTOs;

public class PersonUpdateDto
{
    [Required, MaxLength(50)]
    public string? FirstName { get; set; }
    
    [Required, MaxLength(50)]
    public string? LastName { get; set; }

    [Required, MaxLength(8)]
    public string? Gender { get; set; }

    [Required, Range(1, 120)]
    public int Age { get; set; }

    [Required, MaxLength(50)]
    public string? Address { get; set; }

    [Required, MaxLength(50)]
    public string? Email { get; set; }

    [Required, MaxLength(50)]
    public string? Phone { get; set; }
}