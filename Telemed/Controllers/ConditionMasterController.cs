// Controllers/ConditionMasterController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ConditionMasterController : ControllerBase
{
    private readonly IConditionMasterService _service;

    public ConditionMasterController(IConditionMasterService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [FromBody] CreateConditionMasterDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { id = result.ConditionId }, result);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Provider,Patient")]
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
                error = $"Condition with ID {id} not found."
            });
        return Ok(result);
    }

    // GET api/ConditionMaster/icd/I10
    [HttpGet("icd/{icdCode}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByIcdCode(string icdCode)
    {
        var result = await _service.GetByIcdCodeAsync(icdCode);
        if (result == null)
            return NotFound(new
            {
                error = $"Condition with ICD code '{icdCode}' not found."
            });
        return Ok(result);
    }

    // GET api/ConditionMaster/type/Chronic
    [HttpGet("type/{type}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetByType(string type)
    {
        var validTypes = new[]
        {
            "Chronic", "Acute", "Mental Health",
            "Infectious", "Genetic", "Autoimmune",
            "Neurological", "Cardiovascular", "Other"
        };
        if (!validTypes.Contains(type, StringComparer.OrdinalIgnoreCase))
            return BadRequest(new
            {
                error = "Invalid type.",
                allowed = validTypes
            });

        var result = await _service.GetByTypeAsync(type);
        return Ok(result);
    }

    // GET api/ConditionMaster/search?keyword=hypert
    [HttpGet("search")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> Search([FromQuery] string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return BadRequest(new { error = "Keyword cannot be empty." });

        var result = await _service.SearchAsync(keyword);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        long id, [FromBody] UpdateConditionMasterDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new
            {
                error = $"Condition with ID {id} not found."
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
                error = $"Condition with ID {id} not found."
            });
        return NoContent();
    }
}