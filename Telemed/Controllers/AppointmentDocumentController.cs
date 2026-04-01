// Controllers/AppointmentDocumentController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppointmentDocumentController : ControllerBase
{
    private readonly IAppointmentDocumentService _service;

    public AppointmentDocumentController(IAppointmentDocumentService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> Create(
        [FromBody] CreateAppointmentDocumentDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { id = result.Id }, result);
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
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null)
            return NotFound(new { error = $"Document with ID {id} not found." });
        return Ok(result);
    }

    [HttpGet("appointment/{appointmentId}")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> GetByAppointmentId(long appointmentId)
    {
        var result = await _service.GetByAppointmentIdAsync(appointmentId);
        return Ok(result);
    }

    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> GetByPatientId(long patientId)
    {
        var result = await _service.GetByPatientIdAsync(patientId);
        return Ok(result);
    }

    [HttpGet("filetype/{filetype}")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> GetByFileType(string filetype)
    {
        var validFileTypes = new[]
        {
            "pdf", "jpg", "jpeg", "png",
            "doc", "docx", "xls", "xlsx",
            "dicom", "hl7", "xml", "txt"
        };
        if (!validFileTypes.Contains(filetype.ToLower()))
            return BadRequest(new
            {
                error = "Invalid file type.",
                allowed = validFileTypes
            });

        var result = await _service.GetByFileTypeAsync(filetype);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> Update(
        long id, [FromBody] UpdateAppointmentDocumentDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new { error = $"Document with ID {id} not found." });
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(long id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(new { error = $"Document with ID {id} not found." });
        return NoContent();
    }
}