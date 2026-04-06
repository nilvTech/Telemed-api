using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    // Post Patient
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreatePatientDto dto)
    {
        var result = await _service.CreateAsync(dto); // returns PatientDto
                                                      // Use correct property name PatientId
        return CreatedAtAction(nameof(GetById), new { id = result.PatientId }, result);
    }

    // GET all patients
    [HttpGet]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    // GET patient by ID
    [HttpGet("{id}")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null)
            return NotFound(new { error = $"Patient with ID {id} not found." });
        return Ok(result);
    }

    // UPDATE patient
    [HttpPut("{id}")]
    [Authorize(Roles = "Patient,Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePatientDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new { error = $"Patient with ID {id} not found." });
        return Ok(result);
    }

    // DELETE patient
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(new { error = $"Patient with ID {id} not found." });
        return NoContent();
    }
}