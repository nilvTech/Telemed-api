// Controllers/CarePatientsummaryController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CarePatientsummaryController : ControllerBase
{
    private readonly ICarePatientsummaryService _service;

    public CarePatientsummaryController(
        ICarePatientsummaryService service)
    {
        _service = service;
    }

    // GET api/CarePatientsummary
    [HttpGet]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    // GET api/CarePatientsummary/patient/3
    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetByPatientId(long patientId)
    {
        var result = await _service.GetByPatientIdAsync(patientId);
        if (result == null)
            return NotFound(new
            {
                error = $"Care summary for patient ID {patientId} not found."
            });
        return Ok(result);
    }

    // GET api/CarePatientsummary/gender/Male
    [HttpGet("gender/{gender}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByGender(string gender)
    {
        var validGenders = new[] { "Male", "Female", "Other" };
        if (!validGenders.Contains(gender, StringComparer.OrdinalIgnoreCase))
            return BadRequest(new
            {
                error = "Invalid gender.",
                allowed = validGenders
            });

        var result = await _service.GetByGenderAsync(gender);
        return Ok(result);
    }

    // GET api/CarePatientsummary/condition/Hypertension
    [HttpGet("condition/{condition}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByCondition(string condition)
    {
        if (string.IsNullOrWhiteSpace(condition))
            return BadRequest(new { error = "Condition cannot be empty." });

        var result = await _service.GetByConditionAsync(condition);
        return Ok(result);
    }

    // GET api/CarePatientsummary/with-alerts
    [HttpGet("with-alerts")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetWithAlerts()
    {
        var result = await _service.GetWithAlertsAsync();
        return Ok(result);
    }

    // GET api/CarePatientsummary/active-careplan
    [HttpGet("active-careplan")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetWithActiveCareplan()
    {
        var result = await _service.GetWithActiveCareplanAsync();
        return Ok(result);
    }

    // GET api/CarePatientsummary/overdue-tasks
    [HttpGet("overdue-tasks")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetWithOverdueTasks()
    {
        var result = await _service.GetWithOverdueTasksAsync();
        return Ok(result);
    }

    // GET api/CarePatientsummary/overdue-followups
    [HttpGet("overdue-followups")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetWithOverdueFollowups()
    {
        var result = await _service.GetWithOverdueFollowupsAsync();
        return Ok(result);
    }

    // GET api/CarePatientsummary/careplan-status/Active
    [HttpGet("careplan-status/{status}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByCareplanStatus(string status)
    {
        var validStatuses = new[]
        {
            "Active", "Completed", "OnHold", "Discontinued"
        };
        if (!validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
            return BadRequest(new
            {
                error = "Invalid careplan status.",
                allowed = validStatuses
            });

        var result = await _service.GetByCareplanStatusAsync(status);
        return Ok(result);
    }

    // GET api/CarePatientsummary/search?keyword=john
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