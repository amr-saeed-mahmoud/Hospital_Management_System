
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using UnitsOfWork;

namespace Controllers;

[ApiController]
[Route("api/LabTech")]
public class LabTechController : ControllerBase
{
    private readonly IMainUnit _MainUnit;

    public LabTechController(IMainUnit mainUnit)
    {
        _MainUnit = mainUnit;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("LabTechs", Name = "GetAll")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _MainUnit.LabsTech.GetAllAsync();
        if (result == null)
        {
            return NoContent();
        }
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("LabTechs/{Id}", Name = "GetById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] int Id)
    {
        var result = await _MainUnit.LabsTech.FindByIdAsync(Id);
        if (result == null)
        {
            return NotFound("LabTech not found");
        }
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("LabTechs/{Id}", Name = "UpdateLabTech")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateLabTech([FromRoute] int Id, [FromBody]LabTechDto Info)
    {
        var labTech = await _MainUnit.LabsTech.FindByIdAsync(Id);
        if (labTech == null)
        {
            return NotFound("LabTech not found");
        }
        labTech.Salary = Info.Salary;
        labTech.IsActive = Info.IsActive;
        await _MainUnit.LabsTech.UpdateAsync(labTech);
        return Ok("LabTech updated successfully");
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("LabTechs/{Id}", Name = "DeleteLabTech")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLabTech([FromRoute] int Id)
    {
        if (await _MainUnit.LabsTech.IsExist(l => l.Id == Id) == false)
        {
            return NotFound("LabTech not found");
        }
        await _MainUnit.LabsTech.DeleteAsync(Id);
        return Ok("LabTech deleted successfully");
    }

}