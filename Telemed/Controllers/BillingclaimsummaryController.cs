// Controllers/BillingclaimsummaryController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BillingclaimsummaryController : ControllerBase
{
    private readonly IBillingclaimsummaryService _service;

    public BillingclaimsummaryController(
        IBillingclaimsummaryService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [FromBody] CreateBillingclaimsummaryDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { id = result.Summaryid }, result);
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
                error = $"Billing summary with ID {id} not found."
            });
        return Ok(result);
    }

    // GET api/Billingclaimsummary/claim/3
    [HttpGet("claim/{claimId}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByClaimId(long claimId)
    {
        var result = await _service.GetByClaimIdAsync(claimId);
        return Ok(result);
    }

    // GET api/Billingclaimsummary/patient/3
    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByPatientId(long patientId)
    {
        var result = await _service.GetByPatientIdAsync(patientId);
        return Ok(result);
    }

    // GET api/Billingclaimsummary/provider/1
    [HttpGet("provider/{providerInfoId}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByProviderId(long providerInfoId)
    {
        var result = await _service.GetByProviderIdAsync(providerInfoId);
        return Ok(result);
    }

    // GET api/Billingclaimsummary/status/Paid
    [HttpGet("status/{status}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        var validStatuses = new[]
        {
            "Pending", "Submitted", "InReview",
            "Approved", "Paid", "Denied",
            "Appealed", "Cancelled"
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

    // GET api/Billingclaimsummary/cptcode/99213
    [HttpGet("cptcode/{cptcode}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByCptcode(string cptcode)
    {
        var result = await _service.GetByCptcodeAsync(cptcode);
        return Ok(result);
    }

    // GET api/Billingclaimsummary/daterange?from=2026-01-01&to=2026-04-30
    [HttpGet("daterange")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByDateRange(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to)
    {
        if (from > to)
            return BadRequest(new
            {
                error = "From date cannot be greater than to date."
            });

        var result = await _service.GetByDateRangeAsync(from, to);
        return Ok(result);
    }

    // GET api/Billingclaimsummary/stats
    [HttpGet("stats")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetStats()
    {
        var result = await _service.GetStatsAsync();
        return Ok(result);
    }

    // GET api/Billingclaimsummary/stats/provider/1
    [HttpGet("stats/provider/{providerInfoId}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetStatsByProvider(long providerInfoId)
    {
        var result = await _service.GetStatsByProviderAsync(providerInfoId);
        return Ok(result);
    }

    // GET api/Billingclaimsummary/stats/patient/3
    [HttpGet("stats/patient/{patientId}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetStatsByPatient(long patientId)
    {
        var result = await _service.GetStatsByPatientAsync(patientId);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        long id, [FromBody] UpdateBillingclaimsummaryDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new
            {
                error = $"Billing summary with ID {id} not found."
            });
        return Ok(result);
    }

    // PATCH api/Billingclaimsummary/5/status
    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStatus(
        long id, [FromBody] BillingStatusUpdateDto dto)
    {
        var result = await _service.UpdateStatusAsync(id, dto);
        if (result == null)
            return NotFound(new
            {
                error = $"Billing summary with ID {id} not found."
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
                error = $"Billing summary with ID {id} not found."
            });
        return NoContent();
    }

    // Billing cliam View

    
}