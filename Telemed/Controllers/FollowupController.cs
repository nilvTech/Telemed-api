// Controllers/FollowupController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FollowupController : ControllerBase
{
    private readonly IFollowupService _service;

    public FollowupController(IFollowupService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> Create([FromBody] CreateFollowupDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { id = result.Followupid }, result);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null)
            return NotFound(new { error = $"Follow-up with ID {id} not found." });
        return Ok(result);
    }

    // GET api/Followup/patient/3
    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetByPatientId(long patientId)
    {
        var result = await _service.GetByPatientIdAsync(patientId);
        return Ok(result);
    }

    // GET api/Followup/appointment/5
    [HttpGet("appointment/{appointmentId}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByAppointmentId(long appointmentId)
    {
        var result = await _service.GetByAppointmentIdAsync(appointmentId);
        return Ok(result);
    }

    // GET api/Followup/status/Scheduled
    [HttpGet("status/{status}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        var validStatuses = new[]
        {
            "Scheduled", "Completed", "Cancelled", "NoShow"
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

    // GET api/Followup/type/Phone Call
    [HttpGet("type/{followuptype}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByType(string followuptype)
    {
        var validTypes = new[]
        {
            "Phone Call", "Visit", "Lab Review",
            "Telehealth", "RPM Review", "Medication Review"
        };
        if (!validTypes.Contains(followuptype, StringComparer.OrdinalIgnoreCase))
            return BadRequest(new
            {
                error = "Invalid follow-up type.",
                allowed = validTypes
            });

        var result = await _service.GetByTypeAsync(followuptype);
        return Ok(result);
    }

    // GET api/Followup/overdue
    [HttpGet("overdue")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetOverdue()
    {
        var result = await _service.GetOverdueAsync();
        return Ok(result);
    }

    // GET api/Followup/today
    [HttpGet("today")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetToday() 
    {
        var result = await _service.GetTodayAsync();
        return Ok(result);
    }

    // GET api/Followup/upcoming?days=7
    [HttpGet("upcoming")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetUpcoming([FromQuery] int days = 7)
    {
        if (days <= 0 || days > 90)
            return BadRequest(new
            {
                error = "Days must be between 1 and 90."
            });

        var result = await _service.GetUpcomingAsync(days);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> Update(
        long id, [FromBody] UpdateFollowupDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new { error = $"Follow-up with ID {id} not found." });
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(long id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(new { error = $"Follow-up with ID {id} not found." });
        return NoContent();
    }
}