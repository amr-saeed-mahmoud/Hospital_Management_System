
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnitsOfWork;

namespace Controllers;

[ApiController]
[Route("api/Patient")]
public class PatientController : ControllerBase
{
    private readonly IMainUnit _MainUnit;

    public PatientController(IMainUnit mainUnit)
    {
        _MainUnit = mainUnit;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Patients", Name = "Patients")]
    public async Task<IActionResult> GetAllPatients()
    {
        var Patients = await _MainUnit.Patients.GetAllAsync();
        return Ok(Patients);
    }

    [Authorize(Roles = "Admin, Nurse")]
    [HttpGet("patients/{Id}", Name = "GetPatientById")]
    public async Task<IActionResult> GetPatientById([FromRoute]int Id)
    {
        var Patient  = await _MainUnit.Patients.FindByIdAsync(Id);
        if(Patient == null)
        {
            return NotFound($"Patient with Id :{Id} is not found.");
        }
        return Ok(Patient);
    } 

    [Authorize(Roles = "Admin")]
    [HttpPost("Patients/{Id}", Name = "DeletePatient")]
    public async Task<IActionResult> Delete([FromRoute]int Id)
    {
        var Patient = await _MainUnit.Patients.FindByIdAsync(Id);
        if(Patient == null)
        {
            return NotFound($"Patient with Id: {Id} is not found.");
        }

        bool IsDeleted = await _MainUnit.Patients.DeleteAsync(Id);
        if(!IsDeleted)
        {
            return BadRequest("Patient cannot be deleted because they have connected data in the system.");
        }
        return Ok("patient deleted successfuly.");
    }

    [Authorize(Roles = "Patient")]
    [Authorize(Policy = "OwnProfile")]
    [HttpGet("Patient/{Id}/MedicalRecords", Name = "GetAllMedicalRecordsbyPatientId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllMedicalRecordsbyPatientId([FromRoute] int Id)
    {
        var Records = 
        from appointment in (await _MainUnit.Appointments.FindByExpression(appointmet => appointmet.PatientId == Id))
        join record in (await _MainUnit.MedicalRecords.GetAllAsync()) on appointment.Id equals record.AppointmentId
        select record;
        
        if(!Records.Any())
        {
            return NoContent();
        }
        return Ok(Records);
    }
    
}