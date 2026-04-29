// Controllers/ClaimformController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClaimformController : ControllerBase
{
    private readonly IClaimformService _service;

    public ClaimformController(IClaimformService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> Create(
        [FromBody] CreateClaimformDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { id = result.Claimformid }, result);
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
                error = $"Claim form with ID {id} not found."
            });
        return Ok(result);
    }

    // GET api/Claimform/patient?name=John
    [HttpGet("patient")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByPatientname(
        [FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest(new { error = "Patient name cannot be empty." });

        var result = await _service.GetByPatientnameAsync(name);
        return Ok(result);
    }

    // GET api/Claimform/policy/POL-001
    [HttpGet("policy/{policynumber}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByPolicynumber(string policynumber)
    {
        var result = await _service.GetByPolicynumberAsync(policynumber);
        return Ok(result);
    }

    // GET api/Claimform/diagnosis/I10
    [HttpGet("diagnosis/{diagnosiscode}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByDiagnosiscode(string diagnosiscode)
    {
        var result = await _service.GetByDiagnosiscodeAsync(diagnosiscode);
        return Ok(result);
    }

    // GET api/Claimform/servicedate/2026-04-09
    [HttpGet("servicedate/{servicedate}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByServicedate(DateOnly servicedate)
    {
        var result = await _service.GetByServicedateAsync(servicedate);
        return Ok(result);
    }

    // GET api/Claimform/daterange?from=2026-01-01&to=2026-04-30
    [HttpGet("daterange")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByDateRange(
        [FromQuery] DateOnly from,
        [FromQuery] DateOnly to)
    {
        if (from > to)
            return BadRequest(new
            {
                error = "From date cannot be greater than to date."
            });

        var result = await _service.GetByDateRangeAsync(from, to);
        return Ok(result);
    }

    // GET api/Claimform/search?keyword=john
    [HttpGet("search")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> Search([FromQuery] string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return BadRequest(new { error = "Keyword cannot be empty." });

        var result = await _service.SearchAsync(keyword);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> Update(
        long id, [FromBody] UpdateClaimformDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new
            {
                error = $"Claim form with ID {id} not found."
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
                error = $"Claim form with ID {id} not found."
            });
        return NoContent();
    }
}