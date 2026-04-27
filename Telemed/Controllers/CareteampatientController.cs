// Controllers/CareteampatientController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CareteampatientController : ControllerBase
{
    private readonly ICareteampatientService _service;

    public CareteampatientController(ICareteampatientService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> Create(
        [FromBody] CreateCareteampatientDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { id = result.Careteampatientid }, result);
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
                error = $"Care team patient with ID {id} not found."
            });
        return Ok(result);
    }

    // GET api/Careteampatient/patient/3
    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetByPatientId(long patientId)
    {
        var result = await _service.GetByPatientIdAsync(patientId);
        return Ok(result);
    }

    // GET api/Careteampatient/patient/3/active
    [HttpGet("patient/{patientId}/active")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetActiveByPatientId(long patientId)
    {
        var result = await _service.GetActiveByPatientIdAsync(patientId);
        return Ok(result);
    }

    // GET api/Careteampatient/careteam/1
    [HttpGet("careteam/{careteamId}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByCareteamId(long careteamId)
    {
        var result = await _service.GetByCareteamIdAsync(careteamId);
        return Ok(result);
    }

    // GET api/Careteampatient/careteam/1/active
    [HttpGet("careteam/{careteamId}/active")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetActiveByCareteamId(long careteamId)
    {
        var result = await _service.GetActiveByCareteamIdAsync(careteamId);
        return Ok(result);
    }

    // GET api/Careteampatient/active
    [HttpGet("active")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetActive()
    {
        var result = await _service.GetActiveAsync();
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> Update(
        long id, [FromBody] UpdateCareteampatientDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new
            {
                error = $"Care team patient with ID {id} not found."
            });
        return Ok(result);
    }

    // PATCH api/Careteampatient/5/deactivate
    [HttpPatch("{id}/deactivate")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> Deactivate(long id)
    {
        var success = await _service.DeactivateAsync(id);
        if (!success)
            return NotFound(new
            {
                error = $"Care team patient with ID {id} not found."
            });
        return Ok(new
        {
            message = $"Patient removed from care team successfully."
        });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(long id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(new
            {
                error = $"Care team patient with ID {id} not found."
            });
        return NoContent();
    }
}