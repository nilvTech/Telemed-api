using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProviderController : ControllerBase
{
    private readonly IProviderService _service;

    public ProviderController(IProviderService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> GetById(long id) // long
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null)
            return NotFound(new { error = $"Provider with ID {id} not found." });
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateProviderDto dto)
    {
        var result = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.ProviderId }, // ensure DTO has ProviderId as long
            result
        );
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateProviderDto dto) // long
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new { error = $"Provider with ID {id} not found." });
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(long id) // long
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(new { error = $"Provider with ID {id} not found." });
        return NoContent();
    }
}   