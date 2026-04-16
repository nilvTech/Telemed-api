// Controllers/ClaimController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClaimController : ControllerBase
{
    private readonly IClaimService _service;

    public ClaimController(IClaimService service)
    {
        _service = service;
    }

    // ========== CLAIM ENDPOINTS ==========

    [HttpPost]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> Create([FromBody] CreateClaimDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { id = result.Claimid }, result);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
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
            return NotFound(new { error = $"Claim with ID {id} not found." });
        return Ok(result);
    }

    // GET api/Claim/number/CLM-2026-001
    [HttpGet("number/{claimnumber}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByClaimnumber(string claimnumber)
    {
        var result = await _service.GetByClaimnumberAsync(claimnumber);
        if (result == null)
            return NotFound(new
            {
                error = $"Claim number '{claimnumber}' not found."
            });
        return Ok(result);
    }

    // GET api/Claim/patient/3
    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetByPatientId(long patientId)
    {
        var result = await _service.GetByPatientIdAsync(patientId);
        return Ok(result);
    }

    // GET api/Claim/provider/1
    [HttpGet("provider/{providerInfoId}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByProviderId(long providerInfoId)
    {
        var result = await _service.GetByProviderIdAsync(providerInfoId);
        return Ok(result);
    }

    // GET api/Claim/status/Pending
    [HttpGet("status/{status}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        var validStatuses = new[]
        {
            "Pending", "Submitted", "InReview",
            "Approved", "Paid", "Denied", "Appealed", "Cancelled"
        };
        if (!validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
            return BadRequest(new
            {
                error = "Invalid status.",
                allowed = validStatuses
            });

        var result = await _service.GetByStatusAsync(status);
        return Ok(result);
    }

    // GET api/Claim/payer/Medicare
    [HttpGet("payer/{payer}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByPayer(string payer)
    {
        var result = await _service.GetByPayerAsync(payer);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> Update(
        long id, [FromBody] UpdateClaimDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new { error = $"Claim with ID {id} not found." });
        return Ok(result);
    }

    // PATCH api/Claim/5/status
    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> UpdateStatus(
        long id, [FromBody] ClaimStatusUpdateDto dto)
    {
        var result = await _service.UpdateStatusAsync(id, dto);
        if (result == null)
            return NotFound(new { error = $"Claim with ID {id} not found." });
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(long id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(new { error = $"Claim with ID {id} not found." });
        return NoContent();
    }

    // ========== CLAIM DETAIL ENDPOINTS ==========

    // POST api/Claim/5/details
    [HttpPost("{claimId}/details")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> AddDetail(
        long claimId, [FromBody] CreateClaimDetailDto dto)
    {
        var result = await _service.AddDetailAsync(claimId, dto);
        return Ok(result);
    }

    // PUT api/Claim/details/3
    [HttpPut("details/{detailId}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> UpdateDetail(
        long detailId, [FromBody] UpdateClaimDetailDto dto)
    {
        var result = await _service.UpdateDetailAsync(detailId, dto);
        if (result == null)
            return NotFound(new
            {
                error = $"Claim detail with ID {detailId} not found."
            });
        return Ok(result);
    }

    // DELETE api/Claim/details/3
    [HttpDelete("details/{detailId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteDetail(long detailId)
    {
        var success = await _service.DeleteDetailAsync(detailId);
        if (!success)
            return NotFound(new
            {
                error = $"Claim detail with ID {detailId} not found."
            });
        return NoContent();
    }
}