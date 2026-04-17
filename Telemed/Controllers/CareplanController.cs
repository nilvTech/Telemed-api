// Controllers/CareplanController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CareplanController : ControllerBase
{
    private readonly ICareplanService _service;

    public CareplanController(ICareplanService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> Create([FromBody] CreateCareplanDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { id = result.Careplanid }, result);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
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
            return NotFound(new
            {
                error = $"Care plan with ID {id} not found."
            });
        return Ok(result);
    }

    // GET api/Careplan/patient/3
    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetByPatientId(long patientId)
    {
        var result = await _service.GetByPatientIdAsync(patientId);
        return Ok(result);
    }

    // GET api/Careplan/patient/3/active
    [HttpGet("patient/{patientId}/active")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetActiveByPatientId(long patientId)
    {
        var result = await _service.GetActiveByPatientIdAsync(patientId);
        if (result == null)
            return NotFound(new
            {
                error = $"No active care plan found for patient {patientId}."
            });
        return Ok(result);
    }

    // GET api/Careplan/provider/1
    [HttpGet("provider/{providerInfoId}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByProviderId(long providerInfoId)
    {
        var result = await _service.GetByProviderIdAsync(providerInfoId);
        return Ok(result);
    }

    // GET api/Careplan/status/Active
    [HttpGet("status/{status}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        var validStatuses = new[]
        {
            "Active", "Completed", "OnHold", "Discontinued"
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

    // GET api/Careplan/risklevel/High
    [HttpGet("risklevel/{risklevel}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByRisklevel(string risklevel)
    {
        var validLevels = new[]
        {
            "Low", "Medium", "High", "Very High"
        };
        if (!validLevels.Contains(risklevel, StringComparer.OrdinalIgnoreCase))
            return BadRequest(new
            {
                error = "Invalid risk level.",
                allowed = validLevels
            });

        var result = await _service.GetByRisklevelAsync(risklevel);
        return Ok(result);
    }

    // GET api/Careplan/overdue-review
    [HttpGet("overdue-review")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetOverdueForReview()
    {
        var result = await _service.GetOverdueForReviewAsync();
        return Ok(result);
    }

    // GET api/Careplan/due-today
    [HttpGet("due-today")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetDueForReviewToday()
    {
        var result = await _service.GetDueForReviewTodayAsync();
        return Ok(result);
    }

    // GET api/Careplan/ccm-not-met
    [HttpGet("ccm-not-met")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetCcmNotMet()
    {
        var result = await _service.GetCcmNotMetAsync();
        return Ok(result);
    }

    // PATCH api/Careplan/5/ccm-minutes?minutes=15
    [HttpPatch("{id}/ccm-minutes")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> AddCcmMinutes(
        long id,
        [FromQuery] int minutes,
        [FromQuery] long? updatedby)
    {
        var result = await _service.AddCcmMinutesAsync(id, minutes, updatedby);
        if (result == null)
            return NotFound(new
            {
                error = $"Care plan with ID {id} not found."
            });
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> Update(
        long id, [FromBody] UpdateCareplanDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new
            {
                error = $"Care plan with ID {id} not found."
            });
        return Ok(result);
    }

    // PATCH api/Careplan/5/status
    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> UpdateStatus(
        long id, [FromBody] CareplanStatusUpdateDto dto)
    {
        var result = await _service.UpdateStatusAsync(id, dto);
        if (result == null)
            return NotFound(new
            {
                error = $"Care plan with ID {id} not found."
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
                error = $"Care plan with ID {id} not found."
            });
        return NoContent();
    }
}