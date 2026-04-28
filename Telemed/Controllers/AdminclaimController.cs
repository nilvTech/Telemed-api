// Controllers/AdminclaimController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdminclaimController : ControllerBase
{
    private readonly IAdminclaimService _service;

    public AdminclaimController(IAdminclaimService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [FromBody] CreateAdminclaimDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { id = result.Adminclaimid }, result);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null)
            return NotFound(new
            {
                error = $"Admin claim with ID {id} not found."
            });
        return Ok(result);
    }

    // GET api/Adminclaim/claim/3
    [HttpGet("claim/{claimId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByClaimId(long claimId)
    {
        var result = await _service.GetByClaimIdAsync(claimId);
        return Ok(result);
    }

    // GET api/Adminclaim/patient/3
    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByPatientId(long patientId)
    {
        var result = await _service.GetByPatientIdAsync(patientId);
        return Ok(result);
    }

    // GET api/Adminclaim/provider/1
    [HttpGet("provider/{providerInfoId}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByProviderId(long providerInfoId)
    {
        var result = await _service.GetByProviderIdAsync(providerInfoId);
        return Ok(result);
    }

    // GET api/Adminclaim/appointment/5
    [HttpGet("appointment/{appointmentId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByAppointmentId(long appointmentId)
    {
        var result = await _service.GetByAppointmentIdAsync(appointmentId);
        return Ok(result);
    }

    // GET api/Adminclaim/encounter/1
    [HttpGet("encounter/{encounterId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByEncounterId(int encounterId)
    {
        var result = await _service.GetByEncounterIdAsync(encounterId);
        return Ok(result);
    }

    // GET api/Adminclaim/status/Submitted
    [HttpGet("status/{status}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        var validStatuses = new[]
        {
            "Submitted", "UnderReview", "Approved",
            "Rejected", "Appealed", "Paid", "Closed"
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

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        long id, [FromBody] UpdateAdminclaimDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new
            {
                error = $"Admin claim with ID {id} not found."
            });
        return Ok(result);
    }

    // PATCH api/Adminclaim/5/status
    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStatus(
        long id, [FromBody] AdminclaimStatusUpdateDto dto)
    {
        var result = await _service.UpdateStatusAsync(id, dto);
        if (result == null)
            return NotFound(new
            {
                error = $"Admin claim with ID {id} not found."
            });
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(long id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(new
            {
                error = $"Admin claim with ID {id} not found."
            });
        return NoContent();
    }
}