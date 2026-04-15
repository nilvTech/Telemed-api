using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PatientTaskController : ControllerBase
{
    private readonly IPatientTaskService _service;

    public PatientTaskController(IPatientTaskService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> Create([FromBody] CreatePatientTaskDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Taskid }, result);
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
            return NotFound(new { error = $"Task {id} not found" });

        return Ok(result);
    }

    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetByPatientId(long patientId)
    {
        return Ok(await _service.GetByPatientIdAsync(patientId));
    }

    [HttpGet("provider/{providerId}")]
    public async Task<IActionResult> GetByProviderId(long providerId)
    {
        return Ok(await _service.GetByProviderIdAsync(providerId));
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        return Ok(await _service.GetByStatusAsync(status));
    }

    [HttpGet("priority/{priority}")]
    public async Task<IActionResult> GetByPriority(string priority)
    {
        return Ok(await _service.GetByPriorityAsync(priority));
    }

    [HttpGet("overdue")]
    public async Task<IActionResult> GetOverdue()
    {
        return Ok(await _service.GetOverdueAsync());
    }

    [HttpGet("due-today")]
    public async Task<IActionResult> GetDueToday()
    {
        return Ok(await _service.GetDueTodayAsync());
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateTaskDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> UpdateStatus(long id, [FromBody] TaskStatusUpdateDto dto)
    {
        var result = await _service.UpdateStatusAsync(id, dto);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(long id)
    {
        var success = await _service.DeleteAsync(id);

        if (!success)
            return NotFound();

        return NoContent();
    }
}