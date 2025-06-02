using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Domain.Services;
using Models.DTOs;
using UnitsOfWork;

namespace Controllers;

[ApiController]
[Route("api/Prescription")]
public class PrescriptionController : ControllerBase
{
    private readonly IMainUnit _MainUnit;
    public PrescriptionController(IMainUnit mainUnit)
    {
        _MainUnit = mainUnit;
    }

    [Authorize(Roles = "Doctor, Admin")]
    [HttpGet("Prescriptions", Name = "GetPrescriptions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetPrescriptions()
    {
        var prescriptions = await _MainUnit.Prescriptions.GetAllAsync();
        if(prescriptions == null)
        {
            return NoContent();
        }
        return Ok(prescriptions);
    }

    [Authorize(Roles = "Doctor, Admin")]
    [HttpGet("Prescriptions/MedicalRecord/{Id}", Name = "GetPrescriptionByMedicalRecordId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPrescriptionByMedicalRecordId(int Id)
    {
        if(await _MainUnit.MedicalRecords.IsExist(medicalrecord => medicalrecord.Id == Id) == false)
        {
            return NotFound("Medical Record does not exist");
        }
        var result = await _MainUnit.Prescriptions.FindByExpression(p => p.MedicalRecordId == Id);
        if(result == null)
        {
            return NoContent();
        }
        return Ok(result);
    }

    [Authorize(Roles = "Admin, Doctor")]
    [HttpGet("Prescription/{Id}", Name = "GetPrescriptionById")]
    public async Task<IActionResult> GetPrescriptionById([FromRoute] int Id)
    {
        var prescription = await _MainUnit.Prescriptions.FindByIdAsync(Id);
        if(prescription == null)
        {
            return NotFound("Prescription does not exist");
        }
        return Ok(prescription);
    }

    [Authorize(Roles = "Doctor")]
    [HttpPost("Create", Name = "CreatePrescription")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePrescription(PrescriptionDto Info)
    {
        if(await _MainUnit.MedicalRecords.IsExist(medicalrecord => medicalrecord.Id == Info.MedicalRecordId) == false)
        {
            return BadRequest("Medical Record does not exist");
        }
        
        var Prescription = new Prescription()
        {
            MedicationName = Info.MedicationName,
            Dosage = Info.Dosage,
            Frequency = Info.Frequency,
            StartDate = Info.StartDate,
            EndDate = Info.EndDate,
            SpecialInstructions = Info.SpecialInstructions,
            MedicalRecordId = Info.MedicalRecordId
        };
        await _MainUnit.Prescriptions.AddNew(Prescription);
        return Ok("Prescription Created Successfully");
    }

    [Authorize(Roles = "Doctor")]
    [HttpPatch("Prescription/{Id}", Name = "UpdatePrescription")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePrescription([FromRoute]int Id, [FromBody] PrescriptionDto Info)
    {
        var prescription = await _MainUnit.Prescriptions.FindByIdAsync(Id);
        if(prescription == null)
        {
            return NotFound("Prescription does not exist");
        }
        prescription.MedicationName = Info.MedicationName;
        prescription.Dosage = Info.Dosage;
        prescription.Frequency = Info.Frequency;
        prescription.StartDate = Info.StartDate;
        prescription.EndDate = Info.EndDate;
        prescription.SpecialInstructions = Info.SpecialInstructions;
        prescription.MedicalRecordId = Info.MedicalRecordId;
        await _MainUnit.Prescriptions.UpdateAsync(prescription);
        return Ok("Prescription Updated Successfully");
    }
    
    [Authorize(Roles = "Doctor")]
    [HttpDelete("Prescription/Delete/{Id}", Name = "DeletePrescription")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePrescription([FromRoute]int Id)
    {
        if(await _MainUnit.Prescriptions.IsExist(p => p.Id == Id) == false)
        {
            return NotFound("Prescription does not exist");
        }
        await _MainUnit.Prescriptions.DeleteAsync(Id);
        return Ok("Prescription Deleted Successfully");
    }
}