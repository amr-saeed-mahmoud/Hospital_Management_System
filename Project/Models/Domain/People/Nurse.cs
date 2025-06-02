using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Domain.Services;

namespace Models.Domain.People;


[Table("Nurses")]
public class Nurse : Person
{

    [Required]
    public bool IsActive { get; set; }

    [Required, Range(0, double.MaxValue)]
    public decimal Salary { get; set; }
    
    public int DepartmentId { get; set; }
    [ForeignKey("DepartmentId")]
    public Department? CurrentDepartment { get; set; }
}