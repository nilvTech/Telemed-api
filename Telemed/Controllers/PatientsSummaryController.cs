// Controllers/PatientSummaryController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PatientsSummaryController : ControllerBase
{
    private readonly IPatientsSummaryService _service;

    public PatientsSummaryController(IPatientsSummaryService service)
    {
        _service = service;
    }

    // GET api/PatientSummary
    [HttpGet]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    // GET api/PatientSummary/patient/3
    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetByPatientId(long patientId)
    {
        var result = await _service.GetByPatientIdAsync(patientId);
        if (result == null)
            return NotFound(new
            {
                error = $"Patient summary for ID {patientId} not found."
            });
        return Ok(result);
    }

    // GET api/PatientSummary/gender/Male
    [HttpGet("gender/{gender}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByGender(string gender)
    {
        var validGenders = new[] { "Male", "Female", "Other" };
        if (!validGenders.Contains(gender, StringComparer.OrdinalIgnoreCase))
            return BadRequest(new
            {
                error = "Invalid gender.",
                allowed = validGenders
            });

        var result = await _service.GetByGenderAsync(gender);
        return Ok(result);
    }

    // GET api/PatientSummary/condition/Hypertension
    [HttpGet("condition/{condition}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByCondition(string condition)
    {
        if (string.IsNullOrWhiteSpace(condition))
            return BadRequest(new { error = "Condition cannot be empty." });

        var result = await _service.GetByConditionAsync(condition);
        return Ok(result);
    }

    // GET api/PatientSummary/search?keyword=john
    [HttpGet("search")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> Search([FromQuery] string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return BadRequest(new { error = "Keyword cannot be empty." });

        var result = await _service.SearchAsync(keyword);
        return Ok(result);
    }
}