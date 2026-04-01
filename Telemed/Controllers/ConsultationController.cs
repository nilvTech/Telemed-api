// Controllers/ConsultationController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ConsultationController : ControllerBase
{
    private readonly IConsultationService _service;

    public ConsultationController(IConsultationService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> Create([FromBody] CreateConsultationDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { id = result.Consultationid }, result);
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
            return NotFound(new { error = $"Consultation with ID {id} not found." });
        return Ok(result);
    }

    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> GetByPatientId(long patientId)
    {
        var result = await _service.GetByPatientIdAsync(patientId);
        return Ok(result);
    }

    [HttpGet("provider/{providerId}")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> GetByProviderId(long providerId)
    {
        var result = await _service.GetByProviderIdAsync(providerId);
        return Ok(result);
    }

    [HttpGet("appointment/{appointmentId}")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> GetByAppointmentId(long appointmentId)
    {
        var result = await _service.GetByAppointmentIdAsync(appointmentId);
        return Ok(result);
    }

    [HttpGet("status/{status}")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        var validStatuses = new[]
        {
            "InProgress", "Completed", "Cancelled", "OnHold"
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

    [HttpGet("followups/patient/{patientId}")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> GetFollowUps(long patientId)
    {
        var result = await _service.GetFollowUpsAsync(patientId);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> Update(
        long id, [FromBody] UpdateConsultationDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new { error = $"Consultation with ID {id} not found." });
        return Ok(result);
    }

    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> UpdateStatus(
        long id, [FromBody] ConsultationStatusUpdateDto dto)
    {
        var result = await _service.UpdateStatusAsync(id, dto);
        if (result == null)
            return NotFound(new { error = $"Consultation with ID {id} not found." });
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(long id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(new { error = $"Consultation with ID {id} not found." });
        return NoContent();
    }
}