using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

    // ---------------- GET ALL PROVIDERS ----------------
    // Patients can see all providers (to choose/browse), Admin & Provider also allowed
    [HttpGet]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    // ---------------- GET PROVIDER BY ID ----------------
    [HttpGet("{id}")]
    [Authorize(Roles = "Patient,Provider,Admin")]
    public async Task<IActionResult> GetById(long id)
    {
        // Providers can only view their own profile
        if (User.IsInRole("Provider"))
        {
            var claimValue = User.FindFirst("ReferenceId")?.Value;

            if (!long.TryParse(claimValue, out var tokenProviderId) || tokenProviderId != id)
                return Forbid();
        }

        var result = await _service.GetByIdAsync(id);
        if (result == null)
            return NotFound(new { error = $"Provider with ID {id} not found." });

        return Ok(result);
    }

    // ---------------- CREATE PROVIDER ----------------
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateProviderDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.ProviderId }, result);
    }

    // ---------------- UPDATE PROVIDER ----------------
    [HttpPut("{id}")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateProviderDto dto)
    {
        // Providers can only update their own profile
        if (User.IsInRole("Provider"))
        {
            var claimValue = User.FindFirst("ReferenceId")?.Value;

            if (!long.TryParse(claimValue, out var tokenProviderId) || tokenProviderId != id)
                return Forbid();
        }

        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new { error = $"Provider with ID {id} not found." });

        return Ok(result);
    }

    // ---------------- DELETE PROVIDER ----------------
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(long id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(new { error = $"Provider with ID {id} not found." });

        return NoContent();
    }
}