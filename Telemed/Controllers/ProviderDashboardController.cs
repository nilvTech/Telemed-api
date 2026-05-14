// Controllers/ProviderDashboardController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProviderDashboardController : ControllerBase
{
    private readonly IProviderDashboardService _service;

    public ProviderDashboardController(IProviderDashboardService service)
    {
        _service = service;
    }

    // GET api/ProviderDashboard/1
    // Full dashboard — call this on page load
    [HttpGet("{providerinfoid}")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> GetDashboard(long providerinfoid)
    {
        var result = await _service.GetDashboardAsync(providerinfoid);
        if (result == null)
            return NotFound(new
            {
                error = $"Provider with ID {providerinfoid} not found."
            });
        return Ok(result);
    }

    // GET api/ProviderDashboard/1/stats
    // Only stats cards — for quick refresh
    [HttpGet("{providerinfoid}/stats")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> GetStats(long providerinfoid)
    {
        var result = await _service.GetStatsAsync(providerinfoid);
        return Ok(result);
    }

    // GET api/ProviderDashboard/1/appointments
    // Today's appointments section
    [HttpGet("{providerinfoid}/appointments")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> GetTodaysAppointments(long providerinfoid)
    {
        var result = await _service
            .GetTodaysAppointmentsAsync(providerinfoid);
        return Ok(result);
    }

    // GET api/ProviderDashboard/1/notifications
    // Notifications section
    [HttpGet("{providerinfoid}/notifications")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> GetNotifications(long providerinfoid)
    {
        var result = await _service.GetNotificationsAsync(providerinfoid);
        return Ok(result);
    }

    // GET api/ProviderDashboard/1/consultations
    // Pending consultations section
    [HttpGet("{providerinfoid}/consultations")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> GetPendingConsultations(long providerinfoid)
    {
        var result = await _service
            .GetPendingConsultationsAsync(providerinfoid);
        return Ok(result);
    }

    // GET api/ProviderDashboard/1/alerts
    // Patient alerts section
    [HttpGet("{providerinfoid}/alerts")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> GetPatientAlerts(long providerinfoid)
    {
        var result = await _service.GetPatientAlertsAsync(providerinfoid);
        return Ok(result);
    }
}