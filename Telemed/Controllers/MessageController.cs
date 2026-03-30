using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MessageController : ControllerBase
{
    private readonly IMessageService _service;

    public MessageController(IMessageService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> Create([FromBody] CreateMessageDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { id = result.Messageid }, result);
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
            return NotFound(new { error = $"Message with ID {id} not found." });
        return Ok(result);
    }

    [HttpGet("conversation")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> GetConversation(
        [FromQuery] int patientId,
        [FromQuery] int providerId)
    {
        var result = await _service.GetConversationAsync(patientId, providerId);
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

    [HttpGet("unread")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> GetUnread(
        [FromQuery] int patientId,
        [FromQuery] int providerId)
    {
        var result = await _service.GetUnreadAsync(patientId, providerId);
        return Ok(result);
    }

    [HttpPatch("{id}/read")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var result = await _service.MarkAsReadAsync(id);
        if (result == null)
            return NotFound(new { error = $"Message with ID {id} not found." });
        return Ok(result);
    }

    [HttpPatch("conversation/read-all")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> MarkAllAsRead(
        [FromQuery] int patientId,
        [FromQuery] int providerId)
    {
        var success = await _service.MarkAllAsReadAsync(patientId, providerId);
        if (!success)
            return NotFound(new { error = "No unread messages found." });
        return Ok(new { message = "All messages marked as read." });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(new { error = $"Message with ID {id} not found." });
        return NoContent();
    }
}