using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _service;

    public AppointmentController(IAppointmentService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { id = result.Appointmentid }, result);
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
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null)
            return NotFound(new { error = $"Appointment with ID {id} not found." });
        return Ok(result);
    }

    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> GetByPatientId(int patientId)
    {
        var result = await _service.GetByPatientIdAsync(patientId);
        return Ok(result);
    }

    [HttpGet("provider/{providerId}")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> GetByProviderId(int providerId)
    {
        var result = await _service.GetByProviderIdAsync(providerId);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAppointmentDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new { error = $"Appointment with ID {id} not found." });
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(new { error = $"Appointment with ID {id} not found." });
        return NoContent();
    }
}