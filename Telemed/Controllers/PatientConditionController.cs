// Controllers/PatientConditionController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PatientConditionController : ControllerBase
{
    private readonly IPatientConditionService _service;

    public PatientConditionController(IPatientConditionService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> Create(
        [FromBody] CreatePatientConditionDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { id = result.PatientConditionId }, result);
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
                error = $"Patient condition with ID {id} not found."
            });
        return Ok(result);
    }

    // GET api/PatientCondition/patient/3
    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetByPatientId(long patientId)
    {
        var result = await _service.GetByPatientIdAsync(patientId);
        return Ok(result);
    }

    // GET api/PatientCondition/patient/3/active
    [HttpGet("patient/{patientId}/active")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetActiveByPatientId(long patientId)
    {
        var result = await _service.GetActiveByPatientIdAsync(patientId);
        return Ok(result);
    }

    // GET api/PatientCondition/consultation/1
    [HttpGet("consultation/{consultationId}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByConsultationId(long consultationId)
    {
        var result = await _service.GetByConsultationIdAsync(consultationId);
        return Ok(result);
    }

    // GET api/PatientCondition/provider/1
    [HttpGet("provider/{providerInfoId}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByProviderId(long providerInfoId)
    {
        var result = await _service.GetByProviderIdAsync(providerInfoId);
        return Ok(result);
    }

    // GET api/PatientCondition/status/Active
    [HttpGet("status/{status}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        var validStatuses = new[]
        {
            "Active", "Resolved", "Managed",
            "Inactive", "Suspected"
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

    [HttpPut("{id}")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> Update(
        long id, [FromBody] UpdatePatientConditionDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new
            {
                error = $"Patient condition with ID {id} not found."
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
                error = $"Patient condition with ID {id} not found."
            });
        return NoContent();
    }
}