
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Domain.Enums;
using Models.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using UnitsOfWork;

namespace Controllers;

[ApiController]
[Route("api/Report")]
public class ReportController : ControllerBase
{
    private readonly IMainUnit _MainUnit;
    public ReportController(IMainUnit mainUnit)
    {
        _MainUnit = mainUnit;
    }

    [Authorize(Roles = "Admin, Doctor, Nurse")]
    [HttpGet("Appointments/{Date}" ,Name = "GetAppointmentReportByDate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAppointmentReportByDate([FromRoute] DateOnly Date)
    {
        var appointmentReportDtos = await _MainUnit.Appointments
        .GetAllIncluding(a => a.CurrentDoctor!, a => a.CurrentPatient!)
        .Where(a => a.ScheduledDateTime == Date)
        .Select(a => new AppointmentReportDto
        {
            DoctorName = a.CurrentDoctor!.FirstName + " " + a.CurrentDoctor.LastName,
            PatientName = a.CurrentPatient!.FirstName + " " + a.CurrentPatient.LastName,
            AppointmentStatus = a.Status == 
            AppointmentStatus.Canceled ? "Canceled" :
            a.Status == AppointmentStatus.Completed ? "Completed" :
            a.Status == AppointmentStatus.Scheduled ? "Scheduled" : "Unknown"
        })
        .ToListAsync();

        var document = GenerateAppointmentReport(appointmentReportDtos, Date);
        var pdf = document.GeneratePdf();
        return File(pdf, "application/pdf", "appointments.pdf");
    }
    private IDocument GenerateAppointmentReport(List<AppointmentReportDto> Appoitments, DateOnly Date)
    {
        return Document.Create(container => 
            container.Page(page => {
                page.Size(PageSizes.A3);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(20));

                page.Header()
                .Text($"Appointments for Date: {Date.ToString()}")
                .Bold()
                .FontSize(35)
                .FontColor(Colors.Red.Darken3);

                page.Content()
                .PaddingVertical(2, Unit.Centimetre)
                .Column(x => {
                    x.Spacing(1, Unit.Centimetre);
                    foreach(var a in Appoitments)
                    {
                       
                        x.Item()
                        .Text($"Doctor Name: {a.DoctorName }.");
                        
                        x.Item()
                        .Text($"Patient Name: {a.PatientName}.");
                        
                        x.Item()
                        .Text($"Appointment Status: {a.AppointmentStatus}.");

                    }
                });

                page.Footer()
                .AlignCenter()
                .Text(x => {
                    x.Span("Page ");
                    x.CurrentPageNumber();
                });
            })
        );
    }


    


}