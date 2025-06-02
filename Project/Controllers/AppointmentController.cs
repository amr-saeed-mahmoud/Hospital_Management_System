using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Domain.Enums;
using Models.Domain.Services;
using Models.DTOs;
using UnitsOfWork;

namespace Controllers;

[ApiController]
[Route("api/Appointment")]
public class AppointmentController : ControllerBase
{
    private IMainUnit _MainUnit;
    public AppointmentController(IMainUnit mainUnit)
    {
        _MainUnit = mainUnit;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Appointments", Name = "GetAllAppointments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetllAppointments()
    {
        var Appointments = await _MainUnit.Appointments.GetAllAsync();
        if(Appointments == null)
        {
            return NoContent();
        }
        return Ok(Appointments);
    }

    [Authorize(Roles = "Admin, Patient")]
    [HttpGet("Appointments/{Id}", Name = "FindAppointmentById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> FindAppointmentById([FromRoute]int Id)
    {
        var Appointment = await _MainUnit.Appointments.FindByIdAsync(Id);
        if(Appointment == null)
        {
            return NotFound($"Appointment wiht Id: {Id} is not exist");
        }
        return Ok(Appointment);
    }

    [Authorize(Roles = "Patient")]
    [HttpPost("Create", Name = "CreateAppointment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateNewAppointment(AppointmentDto Info)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest("data is not valid");
        }
        Appointment NewAppointment = new Appointment()
        {
            ScheduledDateTime = Info.ScheduledDateTime,
            Status = AppointmentStatus.Scheduled,
            DoctorId = Info.DoctorId,
            PatientId = Info.PatientId,
        };
        await _MainUnit.Appointments.AddNew(NewAppointment);
        return Ok(new {AppointmentId = NewAppointment.Id, Message = "Appointment Created Successfully."});
    }

    [Authorize(Roles = "Patient")]
    [HttpPatch("Appointments/{Id}", Name = "UpdateAppointmentDate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAppointmentDate([FromRoute]int Id, [FromBody]DateOnly AppointmentDate)
    {
        var Appointment = await _MainUnit.Appointments.FindByIdAsync(Id);
        if(Appointment == null)
        {
            return NotFound($"Appointment with Id: {Id} is not exist.");
        }
        Appointment.ScheduledDateTime = AppointmentDate;
        bool Isupdated = await _MainUnit.Appointments.UpdateAsync(Appointment);
        if(Isupdated)
        {
            return BadRequest("Appointment Date is not updated.");
        }
        return Ok($"new Appointment Date is : {AppointmentDate}");
    }

    [Authorize(Roles = "Patient")]
    [HttpPatch("Appointments/{Id}/Cancle", Name = "CancleAppointment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CancleAppointment([FromRoute]int Id)
    {
        var Appointment = await _MainUnit.Appointments.FindByIdAsync(Id);
        if(Appointment == null)
        {
            return NotFound($"Appointment with Id :{Id} is not found.");
        }
        if(Appointment.Status == AppointmentStatus.Scheduled)
        {
            Appointment.Status = AppointmentStatus.Canceled;
            await _MainUnit.Appointments.UpdateAsync(Appointment);
            return Ok($"Appointment with Id:{Id} is Cancle.");
        }
        return BadRequest("this is applicable in Schedule status only.");
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("Complete/{Id}", Name = "CompleteAppointment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CompleteAppointment([FromRoute] int Id)
    {
        var Appointment = await _MainUnit.Appointments.FindByIdAsync(Id);
        if(Appointment == null)
        {
            return NotFound($"Appointment with id: {Id} is not exists.");
        }
        bool PaymentIsExist = await _MainUnit.Payments.IsExist(bill => bill.AppointmentId == Appointment.Id);
        if(!PaymentIsExist)
        {
            return BadRequest($"to complete the appointment you must pay first.");
        }
        Appointment.Status = AppointmentStatus.Completed;
        await _MainUnit.Appointments.UpdateAsync(Appointment);
        return Ok("the appointment completed successfully.");
    }

    [Authorize(Roles = "Admin, Doctor")]
    [HttpGet("Doctors/{Id}", Name = "GetAllAppointmentByDoctorId")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAppointmentByDoctorId([FromRoute]int Id)
    {
        var Doctor = await _MainUnit.Doctors.FindByIdAsync(Id);
        if(Doctor == null)
        {
            return NotFound($"doctor with Id:{Id} is not found.");
        }
        var Apponitments = await _MainUnit.Appointments.FindByExpression(appointment => appointment.DoctorId ==Id);
        if(Apponitments.Any())
        {
            return Ok(Apponitments);
        }
        return NoContent();
    }

    [Authorize(Roles = "Patient, Admin")]
    [HttpGet("Patients/{Id}", Name = "GetAllAppointmentByPatientId")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAppointmentByPatientId([FromRoute] int Id)
    {
        var Patient = await _MainUnit.Patients.FindByIdAsync(Id);
        if(Patient == null)
        {
            return NotFound($"patient with id :{Id} is not found.");
        }   
        var Appointments = await _MainUnit.Appointments.FindByExpression(appointment => appointment.PatientId == Id);
        if(Appointments.Any())
        {
            return Ok(Appointments);
        }
        return NoContent();
    }
    
}