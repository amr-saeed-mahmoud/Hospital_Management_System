

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Domain.Services;
using Models.DTOs;
using UnitsOfWork;

namespace Controllers;

[ApiController]
[Route("api/Specialization")]
public class SpecializationController : ControllerBase
{
    private readonly IMainUnit _MainUnit;
    public SpecializationController(IMainUnit MainUnit)
    {
        _MainUnit = MainUnit;
    }

    [AllowAnonymous]    
    [HttpGet("Specializations", Name = "GetSpecializations")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetSpecializations()
    {
        var specializations = await _MainUnit.Specializations.GetAllAsync();
        if(specializations == null)
        {
            return NoContent();
        }
        return Ok(specializations);
    }

    [AllowAnonymous]
    [HttpGet("Specializations/{id}", Name = "GetSpecialization")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSpecialization(int id)
    {
        var specialization = await _MainUnit.Specializations.FindByIdAsync(id);
        if(specialization == null)
        {
            return NotFound("Specialization not found");
        }
        return Ok(specialization);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("Create", Name = "CreateSpecialization")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSpeclization(SpeclizationDto Info)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest("Invalid data");
        }
        var specialization = new Specialization
        {
            Name = Info.Name,
            Description = Info.Description,
            DepartmentId = Info.DepartmentId,
            CreatedAt = DateOnly.FromDateTime(DateTime.Now),
            UpdatedAt = DateOnly.FromDateTime(DateTime.Now)
        };

        await _MainUnit.Specializations.AddNew(specialization);
        return Ok("Specialization created successfully");
    }

    [HttpPatch("Specializations/{Id}", Name = "UpdateSpecialization")]
    public async Task<IActionResult> UpdateSpecialization(int Id, SpeclizationDto Info)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest("Invalid data");
        }
        var specialization = await _MainUnit.Specializations.FindByIdAsync(Id);
        if(specialization == null)
        {
            return NotFound("Specialization not found");
        }
        specialization.Name = Info.Name;
        specialization.Description = Info.Description;
        specialization.DepartmentId = Info.DepartmentId;
        specialization.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);

        await _MainUnit.Specializations.UpdateAsync(specialization);
        return Ok("Specialization updated successfully");
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("Specializations/{Id}", Name = "DeleteSpecialization")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSpecialization(int Id)
    {
        if(await _MainUnit.Specializations.IsExist(s => s.Id == Id) == false)
        {
            return NotFound("Specialization not found");
        }
        await _MainUnit.Specializations.DeleteAsync(Id);
        return Ok("Specialization deleted successfully");
    }
}