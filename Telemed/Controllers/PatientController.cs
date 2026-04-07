using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PatientController : ControllerBase
{
    private readonly IPatientService _service;

    public PatientController(IPatientService service)
    {
        _service = service;
    }

    // ---------------- CREATE PATIENT ----------------
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreatePatientDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.PatientId }, result);
    }

    // ---------------- GET ALL PATIENTS ----------------
    [HttpGet]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    // ---------------- GET PATIENT BY ID ----------------
    [HttpGet("{id}")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> GetById(long id)
    {
        // Patients can only view their own record
        if (User.IsInRole("Patient"))
        {
            var claimValue = User.FindFirst("ReferenceId")?.Value;

            if (!long.TryParse(claimValue, out var tokenPatientId) || tokenPatientId != id)
                return Forbid();
        }

        var result = await _service.GetByIdAsync(id);
        if (result == null)
            return NotFound(new { error = $"Patient with ID {id} not found." });

        return Ok(result);
    }

    // ---------------- UPDATE PATIENT ----------------
    [HttpPut("{id}")]
    [Authorize(Roles = "Patient,Admin")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdatePatientDto dto)
    {
        // Patients can only update their own record
        if (User.IsInRole("Patient"))
        {
            var claimValue = User.FindFirst("ReferenceId")?.Value;

            if (!long.TryParse(claimValue, out var tokenPatientId) || tokenPatientId != id)
                return Forbid();
        }

        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new { error = $"Patient with ID {id} not found." });

        return Ok(result);
    }

    // ---------------- DELETE PATIENT ----------------
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(long id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(new { error = $"Patient with ID {id} not found." });

        return NoContent();
    }
}