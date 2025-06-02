using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using UnitsOfWork;

namespace Controllers;

[ApiController]
[Route("api/Nurse")]
public class NurseController : ControllerBase
{
    private readonly IMainUnit _MainUnit;
    public NurseController(IMainUnit MainUnit)
    {
        _MainUnit = MainUnit;
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("Nurses", Name = "GetAllNurses")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllNurses()
    {
        var nurses = await _MainUnit.Nurses.GetAllAsync();
        if(!nurses.Any())
        {
            return NoContent();
        }
        return Ok(nurses);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Nurses/{Id}", Name = "GetNurseById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetNurseById([FromRoute] int Id)
    {
        var Nurse = await _MainUnit.Nurses.FindByIdAsync(Id);
        if(Nurse == null)
        {
            return NotFound($"nurse with id: {Id} was not found");
        }
        return Ok(Nurse);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("Nurses/{Id}", Name = "NurseUpdateDto")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateNurse([FromRoute] int Id, [FromBody] NurseUpdateDto Info)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest("Nurse information is missing");
        }
        var Nurse = await _MainUnit.Nurses.FindByIdAsync(Id);
        if(Nurse == null)
        {
            return NotFound($"Nurse with id: {Id} was not found");
        }
        Nurse.DepartmentId = Info.DepartmentId;
        Nurse.IsActive = Info.IsActive;
        Nurse.Salary = Info.Salary;
        await _MainUnit.Nurses.UpdateAsync(Nurse);
        return Ok($"Nurse with id: {Id} was updated successfully");
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("Nurses/Delete/{Id}", Name = "DeleteNurse")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteNurse(int Id)
    {
        if(await _MainUnit.Nurses.IsExist(n => n.Id == Id) == false)
        {
            return NotFound($"Nurse with id: {Id} was not found");
        }
        await _MainUnit.Nurses.DeleteAsync(Id);
        return Ok($"Nurse with id: {Id} was deleted successfully");
    }

}