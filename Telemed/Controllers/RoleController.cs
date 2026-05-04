// Controllers/RoleController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoleController : ControllerBase
{
    private readonly IRoleService _service;

    public RoleController(IRoleService service)
    {
        _service = service;
    }

    // ========== ROLE CRUD ==========

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateRoleDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { id = result.Roleid }, result);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null)
            return NotFound(new { error = $"Role with ID {id} not found." });
        return Ok(result);
    }

    // GET api/Role/code/ADMIN
    [HttpGet("code/{rolecode}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByRolecode(string rolecode)
    {
        var result = await _service.GetByRolecodeAsync(rolecode);
        if (result == null)
            return NotFound(new
            {
                error = $"Role with code '{rolecode}' not found."
            });
        return Ok(result);
    }

    // GET api/Role/type/Provider
    [HttpGet("type/{roletype}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByRoletype(string roletype)
    {
        var validTypes = new[]
        {
            "Admin", "Provider", "Patient",
            "Biller", "Nurse", "CareManager",
            "Pharmacist", "Receptionist", "Auditor"
        };
        if (!validTypes.Contains(roletype, StringComparer.OrdinalIgnoreCase))
            return BadRequest(new
            {
                error = "Invalid role type.",
                allowed = validTypes
            });

        var result = await _service.GetByRoletypeAsync(roletype);
        return Ok(result);
    }

    // GET api/Role/accesslevel/Full
    [HttpGet("accesslevel/{accesslevel}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByAccesslevel(string accesslevel)
    {
        var validLevels = new[]
        {
            "Full", "High", "Medium", "Limited", "Audit"
        };
        if (!validLevels.Contains(accesslevel, StringComparer.OrdinalIgnoreCase))
            return BadRequest(new
            {
                error = "Invalid access level.",
                allowed = validLevels
            });

        var result = await _service.GetByAccesslevelAsync(accesslevel);
        return Ok(result);
    }

    // GET api/Role/active
    [HttpGet("active")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetActive()
    {
        var result = await _service.GetActiveAsync();
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        long id, [FromBody] UpdateRoleDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new { error = $"Role with ID {id} not found." });
        return Ok(result);
    }

    // PATCH api/Role/5/deactivate
    [HttpPatch("{id}/deactivate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Deactivate(
        long id, [FromQuery] long? updatedby)
    {
        var success = await _service.DeactivateAsync(id, updatedby);
        if (!success)
            return NotFound(new { error = $"Role with ID {id} not found." });
        return Ok(new { message = $"Role {id} deactivated successfully." });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(long id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(new { error = $"Role with ID {id} not found." });
        return NoContent();
    }

    // ========== DASHBOARD VIEW ==========

    // GET api/Role/dashboard
    [HttpGet("dashboard")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetDashboard()
    {
        var result = await _service.GetDashboardAsync();
        return Ok(result);
    }

    // GET api/Role/dashboard/5
    [HttpGet("dashboard/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetDashboardById(long id)
    {
        var result = await _service.GetDashboardByIdAsync(id);
        if (result == null)
            return NotFound(new { error = $"Role with ID {id} not found." });
        return Ok(result);
    }
}