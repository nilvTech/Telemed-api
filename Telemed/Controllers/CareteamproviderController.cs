// Controllers/CareteamproviderController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CareteamproviderController : ControllerBase
{
    private readonly ICareteamproviderService _service;

    public CareteamproviderController(ICareteamproviderService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [FromBody] CreateCareteamproviderDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { id = result.Careteamproviderid }, result);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null)
            return NotFound(new
            {
                error = $"Care team provider with ID {id} not found."
            });
        return Ok(result);
    }

    // GET api/Careteamprovider/careteam/1
    [HttpGet("careteam/{careteamId}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByCareteamId(long careteamId)
    {
        var result = await _service.GetByCareteamIdAsync(careteamId);
        return Ok(result);
    }

    // GET api/Careteamprovider/careteam/1/active
    [HttpGet("careteam/{careteamId}/active")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetActiveByCareteamId(long careteamId)
    {
        var result = await _service.GetActiveByCareteamIdAsync(careteamId);
        return Ok(result);
    }

    // GET api/Careteamprovider/provider/1
    [HttpGet("provider/{providerInfoId}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByProviderId(long providerInfoId)
    {
        var result = await _service.GetByProviderIdAsync(providerInfoId);
        return Ok(result);
    }

    // GET api/Careteamprovider/provider/1/active
    [HttpGet("provider/{providerInfoId}/active")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetActiveByProviderId(long providerInfoId)
    {
        var result = await _service.GetActiveByProviderIdAsync(providerInfoId);
        return Ok(result);
    }

    // GET api/Careteamprovider/role/PCP
    [HttpGet("role/{role}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByRole(string role)
    {
        var validRoles = new[]
        {
            "PCP", "RN", "Care Manager", "Specialist",
            "Pharmacist", "Social Worker",
            "Physical Therapist", "Dietitian"
        };
        if (!validRoles.Contains(role, StringComparer.OrdinalIgnoreCase))
            return BadRequest(new
            {
                error = "Invalid role.",
                allowed = validRoles
            });

        var result = await _service.GetByRoleAsync(role);
        return Ok(result);
    }

    // GET api/Careteamprovider/active
    [HttpGet("active")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetActive()
    {
        var result = await _service.GetActiveAsync();
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        long id, [FromBody] UpdateCareteamproviderDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new
            {
                error = $"Care team provider with ID {id} not found."
            });
        return Ok(result);
    }

    // PATCH api/Careteamprovider/5/deactivate
    [HttpPatch("{id}/deactivate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Deactivate(long id)
    {
        var success = await _service.DeactivateAsync(id);
        if (!success)
            return NotFound(new
            {
                error = $"Care team provider with ID {id} not found."
            });
        return Ok(new
        {
            message = "Provider removed from care team successfully."
        });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(long id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(new
            {
                error = $"Care team provider with ID {id} not found."
            });
        return NoContent();
    }
}