// Controllers/PatientFollowUpController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PatientFollowUpController : ControllerBase
{
    private readonly IPatientFollowUpService _service;

    public PatientFollowUpController(IPatientFollowUpService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> Create(
        [FromBody] CreatePatientFollowUpDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { id = result.Id }, result);
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
            return NotFound(new { error = $"FollowUp with ID {id} not found." });
        return Ok(result);
    }

    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> GetByPatientId(long patientId)
    {
        var result = await _service.GetByPatientIdAsync(patientId);
        return Ok(result);
    }

    [HttpGet("status/{status}")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        var validStatuses = new[]
        {
            "Pending", "Completed", "Cancelled", "Missed"
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

    [HttpGet("upcoming")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> GetUpcoming()
    {
        var result = await _service.GetUpcomingAsync();
        return Ok(result);
    }

    [HttpGet("overdue")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> GetOverdue()
    {
        var result = await _service.GetOverdueAsync();
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> Update(
        long id, [FromBody] UpdatePatientFollowUpDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new { error = $"FollowUp with ID {id} not found." });
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(long id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(new { error = $"FollowUp with ID {id} not found." });
        return NoContent();
    }
}