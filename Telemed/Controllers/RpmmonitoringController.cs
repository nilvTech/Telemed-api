// Controllers/RpmmonitoringController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RpmmonitoringController : ControllerBase
{
    private readonly IRpmmonitoringService _service;

    public RpmmonitoringController(IRpmmonitoringService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> Create(
        [FromBody] CreateRpmmonitoringDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { id = result.Rpmmonitoringid }, result);
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
            return NotFound(new
            {
                error = $"RPM reading with ID {id} not found."
            });
        return Ok(result);
    }

    // GET api/Rpmmonitoring/patient/3
    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetByPatientId(long patientId)
    {
        var result = await _service.GetByPatientIdAsync(patientId);
        return Ok(result);
    }

    // GET api/Rpmmonitoring/patient/3/latest
    [HttpGet("patient/{patientId}/latest")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetLatestByPatient(long patientId)
    {
        var result = await _service.GetLatestByPatientAsync(patientId);
        if (result == null)
            return NotFound(new
            {
                error = $"No RPM readings found for patient {patientId}."
            });
        return Ok(result);
    }

    // GET api/Rpmmonitoring/patient/3/daterange?from=2026-01-01&to=2026-04-09
    [HttpGet("patient/{patientId}/daterange")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetByPatientAndDateRange(
        long patientId,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to)
    {
        if (from > to)
            return BadRequest(new
            {
                error = "From date cannot be greater than to date."
            });

        var result = await _service.GetByPatientIdAndDateRangeAsync(
            patientId, from, to);
        return Ok(result);
    }

    // GET api/Rpmmonitoring/unreviewed
    [HttpGet("unreviewed")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetUnreviewed()
    {
        var result = await _service.GetUnreviewedAsync();
        return Ok(result);
    }

    // GET api/Rpmmonitoring/patient/3/unreviewed
    [HttpGet("patient/{patientId}/unreviewed")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetUnreviewedByPatient(long patientId)
    {
        var result = await _service.GetUnreviewedByPatientAsync(patientId);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> Update(
        long id, [FromBody] UpdateRpmmonitoringDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new
            {
                error = $"RPM reading with ID {id} not found."
            });
        return Ok(result);
    }

    // PATCH api/Rpmmonitoring/5/review
    [HttpPatch("{id}/review")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> MarkReviewed(
        long id, [FromBody] RpmReviewDto dto)
    {
        var result = await _service.MarkReviewedAsync(id, dto);
        if (result == null)
            return NotFound(new
            {
                error = $"RPM reading with ID {id} not found."
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
                error = $"RPM reading with ID {id} not found."
            });
        return NoContent();
    }
}