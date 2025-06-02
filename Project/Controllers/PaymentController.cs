

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Domain.Services;
using Models.DTOs;
using UnitsOfWork;

namespace Controllers;

[ApiController]
[Route("api/Payment")]
public class PaymentController : ControllerBase
{
    private readonly IMainUnit _MainUnit;

    public PaymentController(IMainUnit mainUnit)
    {
        _MainUnit = mainUnit;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Payments", Name = "GetAllPayments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllPayments()
    {
        var Payments = await _MainUnit.Payments.GetAllAsync();
        if (Payments == null)
        {
            return NoContent();
        }

        return Ok(Payments);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Payments/{Id}", Name = "GetPaymentById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPaymentById(int Id)
    {
        var Payment = await _MainUnit.Patients.FindByIdAsync(Id);
        if(Payment == null)
        {
            return NotFound($"the payment with Id:{Id} is not exists.");
        }
        return Ok(Payment);
    }

    [Authorize(Roles = "Patient")]
    [HttpPost("CreateBill", Name = "CreateBill")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateBill(PaymentDto Info)
    {
        var Appointment = await _MainUnit.Appointments.FindByIdAsync(Info.AppointmentId);
        if(Appointment == null)
        {
            return NotFound($"appointment with id: {Info.AppointmentId} is not found.");
        }
        var Bill = new Payment()
        {
            PaymentDate = DateOnly.FromDateTime(DateTime.Now),
            PaymentMethod = Info.PaymentMethod,
            AmountPaid = Info.AmountPaid,
            AppointmentId = Info.AppointmentId,
            AdditionalNotes = Info.AdditionalNotes != null ? Info.AdditionalNotes : null
        };
        await _MainUnit.Payments.AddNew(Bill);
        return Ok("Bill paied successfully.");
    }

}