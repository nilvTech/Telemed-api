// Controllers/SmartgoalController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SmartgoalController : ControllerBase
{
    private readonly ISmartgoalService _service;

    public SmartgoalController(ISmartgoalService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> Create([FromBody] CreateSmartgoalDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { id = result.Smartgoalid }, result);
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
                error = $"Smart goal with ID {id} not found."
            });
        return Ok(result);
    }

    // GET api/Smartgoal/patient/3
    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetByPatientId(long patientId)
    {
        var result = await _service.GetByPatientIdAsync(patientId);
        return Ok(result);
    }

    // GET api/Smartgoal/patient/3/active
    [HttpGet("patient/{patientId}/active")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetActiveByPatientId(long patientId)
    {
        var result = await _service.GetActiveByPatientIdAsync(patientId);
        return Ok(result);
    }

    // GET api/Smartgoal/careplan/1
    [HttpGet("careplan/{careplanId}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetByCareplanId(long careplanId)
    {
        var result = await _service.GetByCareplanIdAsync(careplanId);
        return Ok(result);
    }

    // GET api/Smartgoal/provider/1
    [HttpGet("provider/{providerInfoId}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByProviderId(long providerInfoId)
    {
        var result = await _service.GetByProviderIdAsync(providerInfoId);
        return Ok(result);
    }

    // GET api/Smartgoal/status/Active
    [HttpGet("status/{status}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        var validStatuses = new[]
        {
            "Active", "OnTrack", "Delayed", "Completed", "Cancelled"
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

    // GET api/Smartgoal/type/WEIGHT
    [HttpGet("type/{measurementtype}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetByMeasurementtype(string measurementtype)
    {
        var validTypes = new[]
        {
            "BP_SYSTOLIC", "BP_DIASTOLIC",
            "GLUCOSE", "HBA1C",
            "SPO2", "HEART_RATE",
            "WEIGHT", "BMI",
            "LDL", "DIET", "EXERCISE"
        };
        if (!validTypes.Contains(measurementtype.ToUpper()))
            return BadRequest(new
            {
                error = "Invalid measurement type.",
                allowed = validTypes
            });

        var result = await _service.GetByMeasurementtypeAsync(measurementtype);
        return Ok(result);
    }

    // GET api/Smartgoal/overdue
    [HttpGet("overdue")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetOverdue()
    {
        var result = await _service.GetOverdueAsync();
        return Ok(result);
    }

    // GET api/Smartgoal/at-risk
    [HttpGet("at-risk")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetAtRisk()
    {
        var result = await _service.GetAtRiskAsync();
        return Ok(result);
    }

    // PATCH api/Smartgoal/5/progress
    [HttpPatch("{id}/progress")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> UpdateProgress(
        long id, [FromBody] SmartgoalProgressUpdateDto dto)
    {
        var result = await _service.UpdateProgressAsync(id, dto);
        if (result == null)
            return NotFound(new
            {
                error = $"Smart goal with ID {id} not found."
            });
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> Update(
        long id, [FromBody] UpdateSmartgoalDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new
            {
                error = $"Smart goal with ID {id} not found."
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
                error = $"Smart goal with ID {id} not found."
            });
        return NoContent();
    }
}