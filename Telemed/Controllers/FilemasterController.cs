// Controllers/FilemasterController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FilemasterController : ControllerBase
{
    private readonly IFilemasterService _service;

    public FilemasterController(IFilemasterService service)
    {
        _service = service;
    }

    // POST api/Filemaster
    [HttpPost]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> Create([FromBody] CreateFilemasterDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { id = result.Fileid }, result);
    }

    // GET api/Filemaster
    [HttpGet]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    // GET api/Filemaster/5
    [HttpGet("{id}")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null)
            return NotFound(new { error = $"File with ID {id} not found." });
        return Ok(result);
    }

    // GET api/Filemaster/patient/3
    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> GetByPatientId(long patientId)
    {
        var result = await _service.GetByPatientIdAsync(patientId);
        return Ok(result);
    }

    // GET api/Filemaster/completed
    [HttpGet("completed")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> GetCompleted()
    {
        var result = await _service.GetCompletedAsync();
        return Ok(result);
    }

    // GET api/Filemaster/pending
    [HttpGet("pending")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> GetPending()
    {
        var result = await _service.GetPendingAsync();
        return Ok(result);
    }

    // GET api/Filemaster/filetype/pdf
    [HttpGet("filetype/{filetype}")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> GetByFileType(string filetype)
    {
        var validFileTypes = new[]
        {
            "pdf", "jpg", "jpeg", "png", "gif",
            "doc", "docx", "xls", "xlsx",
            "mp4", "mov", "avi",
            "dicom", "dcm", "hl7", "xml", "txt",
            "zip", "csv"
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

    // PUT api/Filemaster/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> Update(
        long id, [FromBody] UpdateFilemasterDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new { error = $"File with ID {id} not found." });
        return Ok(result);
    }

    // PATCH api/Filemaster/chunk
    // Called after each chunk is uploaded
    [HttpPatch("chunk")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> UpdateChunk([FromBody] UploadChunkDto dto)
    {
        var result = await _service.UpdateChunkAsync(dto);
        if (result == null)
            return NotFound(new { error = $"File with ID {dto.Fileid} not found." });
        return Ok(result);
    }

    // PATCH api/Filemaster/5/complete
    [HttpPatch("{id}/complete")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> MarkComplete(
        long id, [FromQuery] long? updatedby)
    {
        var result = await _service.MarkCompleteAsync(id, updatedby);
        if (result == null)
            return NotFound(new { error = $"File with ID {id} not found." });
        return Ok(result);
    }

    // DELETE api/Filemaster/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(long id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(new { error = $"File with ID {id} not found." });
        return NoContent();
    }

    // POST api/Filemaster/upload
    // Real file upload using IFormFile
    [HttpPost("upload")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload(
        [FromForm] RealFileUploadDto dto)
    {
        var result = await _service.UploadRealFileAsync(dto);
        return Ok(result);
    }
}