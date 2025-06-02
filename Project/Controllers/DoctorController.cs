using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Domain.People;
using Models.DTOs;
using UnitsOfWork;

namespace Controllers;


[ApiController]
[Route("api/Doctor")]
public class DoctorController : ControllerBase
{
    private readonly IMainUnit _MainUnit;

    public DoctorController(IMainUnit mainUnit)
    {
        _MainUnit = mainUnit;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Doctors", Name = "GetAllDoctors")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllDoctors()
    {
        var Doctors = await _MainUnit.Doctors.GetAllAsync();
        if(Doctors == null)
        {
            return NoContent();
        }
        return Ok(Doctors);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Doctors/{Id}", Name = "GetDoctorById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDoctorById([FromRoute] int Id)
    {
        var Doctor = await _MainUnit.Doctors.FindByIdAsync(Id);
        if(Doctor == null)
        {
            return NotFound($"the doctor with Id:{Id} is not exists.");
        }
        return Ok(Doctor);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("Doctors/{Id}", Name = "UpdateDoctorInfo")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateDoctorInfo([FromRoute] int Id, [FromBody]DoctorInfoUpdate Info)
    {
        var Doctor = await _MainUnit.Doctors.FindByIdAsync(Id);
        if(Doctor == null)
        {
            return BadRequest($"doctor with Id:{Id} is not found.");
        }
        Doctor.Salary = Info.Salary;
        Doctor.IsActive = Info.IsActive;
        Doctor.SpecializationId = Info.SpecializationId;
        bool IsUpdated = await _MainUnit.Doctors.UpdateAsync(Doctor);
        if(IsUpdated)
        {
            return Ok("Successful Process.");
        }
        return BadRequest("doctor not updated.");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("Doctors/{Id}/IsActive", Name = "ActiveDoctor")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ActiveDoctor([FromRoute] int Id, [FromBody] bool IsActive)
    {
        var Doctor = await _MainUnit.Doctors.FindByIdAsync(Id);
        if(Doctor == null)
        {
            return BadRequest($"doctor with Id:{Id} is not exists.");
        }
        Doctor.IsActive = IsActive;
        await _MainUnit.Doctors.UpdateAsync(Doctor);
        return Ok($"doctor IsActive: {IsActive}");
    }

}
