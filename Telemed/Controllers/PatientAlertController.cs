// Controllers/PatientAlertController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PatientAlertController : ControllerBase
{
    private readonly IPatientAlertService _service;

    public PatientAlertController(IPatientAlertService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> Create(
        [FromBody] CreatePatientAlertDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { id = result.Alertid }, result);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null)
            return NotFound(new { error = $"Alert with ID {id} not found." });
        return Ok(result);
    }

    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> GetByPatientId(long patientId)
    {
        var result = await _service.GetByPatientIdAsync(patientId);
        return Ok(result);
    }

    [HttpGet("unread/patient/{patientId}")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> GetUnreadByPatient(long patientId)
    {
        var result = await _service.GetUnreadByPatientAsync(patientId);
        return Ok(result);
    }

    [HttpGet("severity/{severity}")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> GetBySeverity(string severity)
    {
        var validSeverities = new[] { "Low", "Medium", "High", "Critical" };
        if (!validSeverities.Contains(severity, StringComparer.OrdinalIgnoreCase))
            return BadRequest(new
            {
                error = "Invalid severity.",
                allowed = validSeverities
            });

        var result = await _service.GetBySeverityAsync(severity);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> Update(
        long id, [FromBody] UpdatePatientAlertDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new { error = $"Alert with ID {id} not found." });
        return Ok(result);
    }

    [HttpPatch("{id}/read")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> MarkAsRead(long id)
    {
        var result = await _service.MarkAsReadAsync(id);
        if (result == null)
            return NotFound(new { error = $"Alert with ID {id} not found." });
        return Ok(result);
    }

    [HttpPatch("{id}/acknowledge")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> Acknowledge(long id)
    {
        var result = await _service.AcknowledgeAsync(id);
        if (result == null)
            return NotFound(new { error = $"Alert with ID {id} not found." });
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(long id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(new { error = $"Alert with ID {id} not found." });
        return NoContent();
    }
}