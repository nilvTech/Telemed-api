using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _service;

    public PaymentController(IPaymentService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Patient,Admin")]
    public async Task<IActionResult> Create([FromBody] CreatePaymentDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { id = result.Paymentid }, result);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
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
            return NotFound(new { error = $"Payment with ID {id} not found." });
        return Ok(result);
    }

    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Patient,Admin")]
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

    [HttpGet("appointment/{appointmentId}")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> GetByAppointmentId(int appointmentId)
    {
        var result = await _service.GetByAppointmentIdAsync(appointmentId);
        return Ok(result);
    }

    [HttpGet("status/{status}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        var validStatuses = new[] { "Pending", "Paid", "Failed" };
        if (!validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
            return BadRequest(new { error = "Invalid status. Allowed: Pending, Paid, Failed." });

        var result = await _service.GetByStatusAsync(status);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePaymentDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new { error = $"Payment with ID {id} not found." });
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(new { error = $"Payment with ID {id} not found." });
        return NoContent();
    }
}