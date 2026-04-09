// Controllers/ProviderGroupController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProviderGroupController : ControllerBase
{
    private readonly IProviderGroupService _service;

    public ProviderGroupController(IProviderGroupService service)
    {
        _service = service;
    }

    // ===================== GROUP ENDPOINTS =====================

    // POST api/ProviderGroup
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateGroup(
        [FromBody] CreateProviderGroupDto dto)
    {
        var result = await _service.CreateGroupAsync(dto);
        return CreatedAtAction(nameof(GetGroupById),
            new { groupId = result.GroupId }, result);
    }

    // GET api/ProviderGroup
    [HttpGet]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetAllGroups()
    {
        var result = await _service.GetAllGroupsAsync();
        return Ok(result);
    }

    // GET api/ProviderGroup/5
    [HttpGet("{groupId}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetGroupById(long groupId)
    {
        var result = await _service.GetGroupByIdAsync(groupId);
        if (result == null)
            return NotFound(new
            {
                error = $"Provider group with ID {groupId} not found."
            });
        return Ok(result);
    }

    // GET api/ProviderGroup/speciality/Cardiology
    [HttpGet("speciality/{speciality}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetGroupsBySpeciality(string speciality)
    {
        var result = await _service.GetGroupsBySpecialityAsync(speciality);
        return Ok(result);
    }

    // GET api/ProviderGroup/state/CA
    // Important for US telemedicine state license compliance
    [HttpGet("state/{state}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetGroupsByState(string state)
    {
        var result = await _service.GetGroupsByStateAsync(state);
        return Ok(result);
    }

    // GET api/ProviderGroup/active
    [HttpGet("active")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetActiveGroups()
    {
        var result = await _service.GetActiveGroupsAsync();
        return Ok(result);
    }

    // PUT api/ProviderGroup/5
    [HttpPut("{groupId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateGroup(
        long groupId,
        [FromBody] UpdateProviderGroupDto dto)
    {
        var result = await _service.UpdateGroupAsync(groupId, dto);
        if (result == null)
            return NotFound(new
            {
                error = $"Provider group with ID {groupId} not found."
            });
        return Ok(result);
    }

    // PATCH api/ProviderGroup/5/deactivate
    [HttpPatch("{groupId}/deactivate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeactivateGroup(
        long groupId,
        [FromQuery] long? updatedby)
    {
        var success = await _service.DeactivateGroupAsync(groupId, updatedby);
        if (!success)
            return NotFound(new
            {
                error = $"Provider group with ID {groupId} not found."
            });
        return Ok(new { message = $"Group {groupId} deactivated successfully." });
    }

    // DELETE api/ProviderGroup/5
    [HttpDelete("{groupId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteGroup(long groupId)
    {
        var success = await _service.DeleteGroupAsync(groupId);
        if (!success)
            return NotFound(new
            {
                error = $"Provider group with ID {groupId} not found."
            });
        return NoContent();
    }

    // ===================== MEMBER ENDPOINTS =====================

    // POST api/ProviderGroup/members
    [HttpPost("members")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddMember(
        [FromBody] AddProviderGroupMemberDto dto)
    {
        var result = await _service.AddMemberAsync(dto);
        return CreatedAtAction(nameof(GetMemberById),
            new { memberId = result.GroupMemberId }, result);
    }

    // GET api/ProviderGroup/5/members
    [HttpGet("{groupId}/members")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetMembersByGroupId(long groupId)
    {
        var result = await _service.GetMembersByGroupIdAsync(groupId);
        return Ok(result);
    }

    // GET api/ProviderGroup/members/3
    [HttpGet("members/{memberId}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetMemberById(long memberId)
    {
        var result = await _service.GetMemberByIdAsync(memberId);
        if (result == null)
            return NotFound(new
            {
                error = $"Group member with ID {memberId} not found."
            });
        return Ok(result);
    }

    // GET api/ProviderGroup/provider/5/groups
    // Get all groups a provider belongs to
    [HttpGet("provider/{providerInfoId}/groups")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetGroupsByProvider(long providerInfoId)
    {
        var result = await _service.GetGroupsByProviderIdAsync(providerInfoId);
        return Ok(result);
    }

    // PUT api/ProviderGroup/members/3
    [HttpPut("members/{memberId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateMember(
        long memberId,
        [FromBody] UpdateProviderGroupMemberDto dto)
    {
        var result = await _service.UpdateMemberAsync(memberId, dto);
        if (result == null)
            return NotFound(new
            {
                error = $"Group member with ID {memberId} not found."
            });
        return Ok(result);
    }

    // DELETE api/ProviderGroup/members/3
    [HttpDelete("members/{memberId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveMember(
        long memberId,
        [FromQuery] long? updatedby)
    {
        var success = await _service.RemoveMemberAsync(memberId, updatedby);
        if (!success)
            return NotFound(new
            {
                error = $"Group member with ID {memberId} not found."
            });
        return Ok(new { message = "Member removed from group successfully." });
    }
}