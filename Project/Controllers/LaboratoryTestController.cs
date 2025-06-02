
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Domain.Services;
using Models.DTOs;
using UnitsOfWork;

namespace Controllers;

[ApiController]
[Route("api/LaboratoryTest")]
public class LaboratoryTestController : ControllerBase
{
    private readonly IMainUnit _MainUnit;
    public LaboratoryTestController(IMainUnit mainUnit)
    {
        _MainUnit = mainUnit;
    }

    [Authorize(Roles = "LabTech")]
    [HttpGet("LaboratoryTests", Name = "GetAllLaboratoryTests")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllLaboratoryTests()
    {
        var laboratoryTests = await _MainUnit.LaboratoryTests.GetAllAsync();
        if(laboratoryTests == null)
        {
            return NoContent();
        }
        return Ok(laboratoryTests);
    }

    [Authorize(Roles = "LabTech")]
    [HttpGet("LaboratoryTests/{Id}", Name = "GetLaboratoryTestById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLaboratoryTestById([FromRoute] int Id)
    {
        var laboratoryTest = await _MainUnit.LaboratoryTests.FindByIdAsync(Id);
        if(laboratoryTest == null)
        {
            return NotFound("Laboratory Test not found");
        }
        return Ok(laboratoryTest);
    }

    [Authorize(Roles = "LabTech")]
    [HttpPost("LaboratoryTests/Create", Name = "CreateLaboratoryTest")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateLaboratoryTest(LaboratoryTestDto Info)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest("Invalid model object");
        }
        LaboratoryTest entity = new LaboratoryTest()
        {
            Name = Info.Name,
            Price = Info.Price,
            Result = Info.Result,
            ApplicationDate = Info.ApplicationDate,
            ReceiptDate = Info.ReceiptDate,
            PatientId = Info.PatientId,
            CreatedById = Info.CreatedById
        };
        await _MainUnit.LaboratoryTests.AddNew(entity);
        return Ok("Laboratory Test created successfully");
    }

    [Authorize(Roles = "LabTech")]
    [HttpPatch("LaboratoryTests/{Id}", Name = "UpdateLaboratoryTest")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateLaboratoryTest([FromRoute] int Id, LaboratoryTestDto Info)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest("Invalid model object");
        }
        
        var entity = await _MainUnit.LaboratoryTests.FindByIdAsync(Id);
        if(entity == null)
        {
            return NotFound("Laboratory Test not found");
        }

        entity.Name = Info.Name;
        entity.Price = Info.Price;
        entity.Result = Info.Result;
        entity.ApplicationDate = Info.ApplicationDate;
        entity.ReceiptDate = Info.ReceiptDate;
        entity.PatientId = Info.PatientId;
        entity.CreatedById = Info.CreatedById;

        await _MainUnit.LaboratoryTests.UpdateAsync(entity);
        return Ok("Laboratory Test created successfully");
    }

    [Authorize(Roles = "LabTech")]
    [HttpDelete("LaboratoryTests/{Id}", Name = "DeleteLaboratoryTest")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLaboratoryTest([FromRoute] int Id)
    {
        if(await _MainUnit.LaboratoryTests.IsExist(l => l.Id == Id) == false)
        {
            return NotFound("Laboratory Test not found");
        }
        await _MainUnit.LaboratoryTests.DeleteAsync(Id);
        return Ok("Laboratory Test deleted successfully");
    }

    [Authorize(Roles = "LabTech")]
    [HttpGet("LaboratoryTests/Patient/{Id}", Name = "GetLaboratoryTestsByPatientId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetLaboratoryTestsByPatientId([FromRoute] int Id)
    {
        var laboratoryTests = await _MainUnit.LaboratoryTests.FindByExpression(l => l.PatientId == Id);
        if(laboratoryTests == null)
        {
            return NoContent();
        }
        return Ok(laboratoryTests);
    }

    [Authorize(Roles = "LabTech")]
    [HttpGet("LaboratoryTests/LabTech/{Id}", Name = "GetLaboratoryTestsByLabTechId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetLaboratoryTestsByLabTechId([FromRoute] int Id)
    {
        var laboratoryTests = await _MainUnit.LaboratoryTests.FindByExpression(l => l.CreatedById == Id);
        if(laboratoryTests == null)
        {
            return NoContent();
        }
        return Ok(laboratoryTests);
    }

}