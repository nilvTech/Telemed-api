// Controllers/PatientDashboardController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PatientDashboardController : ControllerBase
{
    private readonly IPatientDashboardService _service;

    public PatientDashboardController(
        IPatientDashboardService service)
    {
        _service = service;
    }

    // GET api/PatientDashboard
    [HttpGet]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    // GET api/PatientDashboard/patient/3
    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetByPatientId(long patientId)
    {
        var result = await _service.GetByPatientIdAsync(patientId);
        if (result == null)
            return NotFound(new
            {
                error = $"Dashboard for patient ID {patientId} not found."
            });
        return Ok(result);
    }

    // GET api/PatientDashboard/appointments-today
    [HttpGet("appointments-today")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetWithAppointmentsToday()
    {
        var result = await _service.GetWithAppointmentsTodayAsync();
        return Ok(result);
    }

    // GET api/PatientDashboard/pending-claims
    [HttpGet("pending-claims")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetWithPendingClaims()
    {
        var result = await _service.GetWithPendingClaimsAsync();
        return Ok(result);
    }

    // GET api/PatientDashboard/notifications
    [HttpGet("notifications")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetWithUnreadNotifications()
    {
        var result = await _service.GetWithUnreadNotificationsAsync();
        return Ok(result);
    }

    // GET api/PatientDashboard/active-sessions
    [HttpGet("active-sessions")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetWithActiveVideoSessions()
    {
        var result = await _service.GetWithActiveVideoSessionsAsync();

        return Ok(result);
    }

    // GET api/PatientDashboard/search?keyword=john
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