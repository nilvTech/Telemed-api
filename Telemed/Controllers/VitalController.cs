using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VitalController : ControllerBase
{
    private readonly IVitalService _service;

    public VitalController(IVitalService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> Create([FromBody] CreateVitalDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { id = result.Vitalsid }, result);
    }

    [HttpGet]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null)
            return NotFound(new { error = $"Vital with ID {id} not found." });
        return Ok(result);
    }

    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> GetByPatientId(int patientId)
    {
        var result = await _service.GetByPatientIdAsync(patientId);
        return Ok(result);
    }

    [HttpGet("encounter/{encounterId}")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> GetByEncounterId(int encounterId)
    {
        var result = await _service.GetByEncounterIdAsync(encounterId);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVitalDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new { error = $"Vital with ID {id} not found." });
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(new { error = $"Vital with ID {id} not found." });
        return NoContent();
    }
}