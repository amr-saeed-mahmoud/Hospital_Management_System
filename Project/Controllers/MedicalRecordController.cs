using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Domain.Services;
using Models.DTOs;
using UnitsOfWork;

namespace Controllers;

[ApiController]
[Route("api/MedicalRecord")]
public class MedicalRecordController : ControllerBase
{
    private readonly IMainUnit _MainUnit;
    public MedicalRecordController(IMainUnit mainUnit)
    {
        _MainUnit = mainUnit;
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("Records", Name = "GetMedicalRecords")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetMedicalRecords()
    {
        var Records = await _MainUnit.MedicalRecords.GetAllAsync();
        if(!Records.Any())
        {
            return NoContent();
        }
        return Ok(Records);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Records/{Id}", Name = "GetMedicalRecordsById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMedicalRecordsById([FromRoute] int Id)
    {
        var Record = await _MainUnit.MedicalRecords.FindByIdAsync(Id);
        if(Record == null)
        {
            return NotFound($"medical record with Id: {Id} is not found.");
        }
        return Ok(Record);
    }

    [Authorize(Roles = "Doctor")]
    [HttpPost("Create", Name = "CreateMedicalRecord")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateMedicalRecord(MedicalRecordDto Info)
    {
        var Record = new MedicalRecord();
        
        if(await _MainUnit.Appointments.IsExist(a => a.Id == Info.AppointmentId) == false)
        {
            return BadRequest("appointment id is not exists.");
        }
        Record.Diagnosis = Info.Diagnosis;
        Record.VisitDescription = Info.VisitDescription;
        Record.AdditionalNotes = Info.AdditionalNotes;
        Record.AppointmentId = Info.AppointmentId;
        await _MainUnit.MedicalRecords.AddNew(Record);
        return Ok($"Record Created Successfully and his Id : {Record.Id}");
    }

    [Authorize(Roles = "Doctor")]
    [HttpPatch("MedicalRecords/{Id}", Name = "UpdateMedicalRecord")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateMedicalRecord([FromRoute] int Id, [FromBody]MedicalRecordDto Info)
    {
        var Record = await _MainUnit.MedicalRecords.FindByIdAsync(Id);
        if(Record == null)
        {
            return NotFound($"Medical record with id: {Id} is not exists.");
        }
        if(await _MainUnit.Appointments.IsExist(a => a.Id == Info.AppointmentId) == false)
        {
            return BadRequest("appointment id is not exists.");
        }
        Record.Diagnosis = Info.Diagnosis;
        Record.VisitDescription = Info.VisitDescription;
        Record.AdditionalNotes = Info.AdditionalNotes;
        Record.AppointmentId = Info.AppointmentId;
        await _MainUnit.MedicalRecords.UpdateAsync(Record);
        return Ok($"Record Updated Successfully");
    }

    [Authorize(Roles = "Doctor")]
    [HttpDelete("Delete/{Id}", Name = "DeleteMedicalRecord")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMedicalRecord([FromRoute] int Id)
    {
        var Record = await _MainUnit.MedicalRecords.FindByIdAsync(Id);
        if(await _MainUnit.MedicalRecords.IsExist(m => m.Id == Id) == false)
        {
            return NotFound($"medical record with Id: {Id} is not exists.");
        }
        await _MainUnit.MedicalRecords.DeleteAsync(Id);
        return Ok("record deleted successfully.");
    }

}