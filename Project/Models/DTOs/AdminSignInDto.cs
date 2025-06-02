namespace Models.DTOs;


public class AdminSingInDto : PersonDto
{
    public bool IsActive { get; set; }
    public decimal Salary { get; set; }
}