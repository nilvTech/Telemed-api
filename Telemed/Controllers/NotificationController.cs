using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _service;

    public NotificationController(INotificationService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> Create([FromBody] CreateNotificationDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { id = result.Notificationid }, result);
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
            return NotFound(new { error = $"Notification with ID {id} not found." });
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

    [HttpGet("unread/patient/{patientId}")]
    [Authorize(Roles = "Patient,Admin")]
    public async Task<IActionResult> GetUnreadByPatient(int patientId)
    {
        var result = await _service.GetUnreadByPatientAsync(patientId);
        return Ok(result);
    }

    [HttpGet("unread/provider/{providerId}")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> GetUnreadByProvider(int providerId)
    {
        var result = await _service.GetUnreadByProviderAsync(providerId);
        return Ok(result);
    }

    [HttpPatch("{id}/read")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var result = await _service.MarkAsReadAsync(id);
        if (result == null)
            return NotFound(new { error = $"Notification with ID {id} not found." });
        return Ok(result);
    }

    [HttpPatch("read-all/patient/{patientId}")]
    [Authorize(Roles = "Patient,Admin")]
    public async Task<IActionResult> MarkAllAsReadByPatient(int patientId)
    {
        var success = await _service.MarkAllAsReadByPatientAsync(patientId);
        if (!success)
            return NotFound(new { error = "No unread notifications found." });
        return Ok(new { message = "All notifications marked as read." });
    }

    [HttpPatch("read-all/provider/{providerId}")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> MarkAllAsReadByProvider(int providerId)
    {
        var success = await _service.MarkAllAsReadByProviderAsync(providerId);
        if (!success)
            return NotFound(new { error = "No unread notifications found." });
        return Ok(new { message = "All notifications marked as read." });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(new { error = $"Notification with ID {id} not found." });
        return NoContent();
    }
}