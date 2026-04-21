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
    // Step 1 of chunked upload — register file metadata
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

    // POST api/Filemaster/upload
    // Direct full file upload — splits into chunks automatically
    // Saves all chunks to DB Pdfcontent field
    [HttpPost("upload")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload([FromForm] RealFileUploadDto dto)
    {
        var result = await _service.UploadRealFileAsync(dto);
        return Ok(result);
    }

    // POST api/Filemaster/chunk
    // Step 2 of chunked upload — upload one chunk at a time
    // Each chunk saved to DB Pdfcontent list
    [HttpPost("chunk")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadChunk([FromForm] UploadChunkDto dto)
    {
        var result = await _service.UploadChunkAsync(dto);
        if (result == null)
            return NotFound(new { error = $"File with ID {dto.Fileid} not found." });
        return Ok(result);
    }

    // GET api/Filemaster/5/download
    // Download file — merges all DB chunks into single file
    [HttpGet("{id}/download")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> Download(long id)
    {
        var result = await _service.DownloadFileAsync(id);
        if (result == null)
            return NotFound(new { error = $"File with ID {id} not found." });

        // Determine content type
        var contentType = result.Filetype?.ToLower() switch
        {
            "pdf" => "application/pdf",
            "jpg" or "jpeg" => "image/jpeg",
            "png" => "image/png",
            "gif" => "image/gif",
            "doc" => "application/msword",
            "docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "xls" => "application/vnd.ms-excel",
            "xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "mp4" => "video/mp4",
            "txt" => "text/plain",
            "xml" => "application/xml",
            "csv" => "text/csv",
            "zip" => "application/zip",
            _ => "application/octet-stream"
        };

        // Return file as downloadable response
        return File(
            result.Filedata!,
            contentType,
            result.Filename);
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
    // ================= FILE UPLOAD =================

    // POST api/ClinicalOrder/orders/{orderId}/upload-file
    [HttpPost("orders/{orderId}/upload-file")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> UploadFile(long orderId, IFormFile file)
    {
        var result = await _service.UploadOrderFileAsync(orderId, file);
        return Ok(result);
    }

    // GET api/ClinicalOrder/orders/{orderId}/files
    [HttpGet("orders/{orderId}/files")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetFiles(long orderId)
    {
        var result = await _service.GetFilesByOrderIdAsync(orderId);
        return Ok(result);
    }

}