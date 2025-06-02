using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Domain.Services;
using Models.DTOs;
using UnitsOfWork;

namespace Controllers;


[ApiController]
[Route("api/Department")]
public class DepartmentController : ControllerBase
{
    private readonly IMainUnit _MainUnit;
    public DepartmentController(IMainUnit mainUnit)
    {
        _MainUnit = mainUnit;
    }

    [AllowAnonymous]
    [HttpGet("Departments", Name = "GetAllDepartments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllDepartments()
    {
        var departments = await _MainUnit.Departments.GetAllAsync();
        if(!departments.Any())
        {
            return NoContent();
        }
        return Ok(departments);
    }

    [AllowAnonymous]
    [HttpGet("Departments/{Id}", Name = "GetDepartment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDepartment([FromRoute] int Id)
    {
        var department = await _MainUnit.Departments.FindByIdAsync(Id);
        if(department == null)
        {
            return NotFound("Department not found");
        }
        return Ok(department);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("Departments", Name = "CreateDepartment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDepartment([FromBody] DepartmentDto Info)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest("Department information is missing");
        }
        var department = new Department
        {
            Name = Info.Name,
            Description = Info.Description,
            CreatedAt = DateOnly.FromDateTime(DateTime.Now),
            UpdatedAt = DateOnly.FromDateTime(DateTime.Now)
        };
        await _MainUnit.Departments.AddNew(department);
        return Ok("Department created successfully");
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("Departments/{Id}", Name = "UpdateDepartment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDepartment([FromRoute] int Id, [FromBody] DepartmentDto Info)
    {
        var department = await _MainUnit.Departments.FindByIdAsync(Id);
        if(department == null)
        {
            return NotFound("Department not found");
        }
        department.Name = Info.Name;
        department.Description = Info.Description;
        department.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);
        await _MainUnit.Departments.UpdateAsync(department);
        return Ok("Department updated successfully");
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("Departments/Delete/{Id}", Name = "DeleteDepartment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDepartment([FromRoute] int Id)
    {
        if(await _MainUnit.Departments.IsExist(d => d.Id == Id) == false)
        {
            return NotFound("Department not found");
        }
        await _MainUnit.Departments.DeleteAsync(Id);
        return Ok("Department deleted successfully");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Department{Id}/Doctors", Name = "GetDoctorsByDepartemntId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetDoctorsByDepartemntId([FromRoute] int Id)
    {
        var Speclizations = await _MainUnit.Specializations.FindByExpression(s => s.DepartmentId == Id);
        var Doctors = await _MainUnit.Doctors.FindByExpression(d => Speclizations.Any(s => s.Id == d.SpecializationId));
        if(!Doctors.Any())
        {
            return NoContent();
        }
        return Ok(Doctors);
    }
}