// Controllers/PatientAppointmentDashboardController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PatientAppointmentDashboardController : ControllerBase
{
    private readonly IPatientAppointmentDashboardService _service;

    public PatientAppointmentDashboardController(
        IPatientAppointmentDashboardService service)
    {
        _service = service;
    }

    // GET api/PatientAppointmentDashboard
    [HttpGet]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    // GET api/PatientAppointmentDashboard/patient/3
    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetByPatientId(long patientId)
    {
        var result = await _service.GetByPatientIdAsync(patientId);
        return Ok(result);
    }

    // GET api/PatientAppointmentDashboard/appointment/5
    [HttpGet("appointment/{appointmentId}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetByAppointmentId(long appointmentId)
    {
        var result = await _service.GetByAppointmentIdAsync(appointmentId);
        if (result == null)
            return NotFound(new
            {
                error = $"Appointment with ID {appointmentId} not found."
            });
        return Ok(result);
    }

    // GET api/PatientAppointmentDashboard/provider/1
    [HttpGet("provider/{providerId}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByProviderId(long providerId)
    {
        var result = await _service.GetByProviderIdAsync(providerId);
        return Ok(result);
    }

    // GET api/PatientAppointmentDashboard/today
    [HttpGet("today")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetToday()
    {
        var result = await _service.GetTodayAsync();
        return Ok(result);
    }

    // GET api/PatientAppointmentDashboard/upcoming
    [HttpGet("upcoming")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetUpcoming()
    {
        var result = await _service.GetUpcomingAsync();
        return Ok(result);
    }

    // GET api/PatientAppointmentDashboard/past
    [HttpGet("past")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetPast()
    {
        var result = await _service.GetPastAsync();
        return Ok(result);
    }

    // GET api/PatientAppointmentDashboard/status/Booked
    [HttpGet("status/{status}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        var validStatuses = new[]
        {
            "Booked", "Confirmed", "InProgress",
            "Completed", "Cancelled", "NoShow"
        };
        if (!validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
            return BadRequest(new
            {
                error = "Invalid status.",
                allowed = validStatuses
            });

        var result = await _service.GetByStatusAsync(status);
        return Ok(result);
    }

    // GET api/PatientAppointmentDashboard/visitmode/Telemedicine
    [HttpGet("visitmode/{visitmode}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByVisitmode(string visitmode)
    {
        var validModes = new[]
        {
            "Telemedicine", "InPerson", "Phone", "HomeVisit"
        };
        if (!validModes.Contains(visitmode, StringComparer.OrdinalIgnoreCase))
            return BadRequest(new
            {
                error = "Invalid visit mode.",
                allowed = validModes
            });

        var result = await _service.GetByVisitmodeAsync(visitmode);
        return Ok(result);
    }

    // GET api/PatientAppointmentDashboard/telemedicine
    [HttpGet("telemedicine")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetTelemedicine()
    {
        var result = await _service.GetTelemedicineAsync();
        return Ok(result);
    }

    // GET api/PatientAppointmentDashboard/recordings
    [HttpGet("recordings")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetWithRecordings()
    {
        var result = await _service.GetWithRecordingsAsync();
        return Ok(result);
    }

    // GET api/PatientAppointmentDashboard/daterange?from=2026-01-01&to=2026-04-30
    [HttpGet("daterange")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByDateRange(
        [FromQuery] DateOnly from,
        [FromQuery] DateOnly to)
    {
        if (from > to)
            return BadRequest(new
            {
                error = "From date cannot be greater than to date."
            });

        var result = await _service.GetByDateRangeAsync(from, to);
        return Ok(result);
    }

    // GET api/PatientAppointmentDashboard/search?keyword=john
    [HttpGet("search")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> Search([FromQuery] string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return BadRequest(new { error = "Keyword cannot be empty." });

        var result = await _service.SearchAsync(keyword);
        return Ok(result);
    }
}