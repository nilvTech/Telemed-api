// Controllers/ClinicalOrderController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClinicalOrderController : ControllerBase
{
    private readonly IClinicalOrderService _service;

    public ClinicalOrderController(IClinicalOrderService service)
    {
        _service = service;
    }

    // ========== MASTER ENDPOINTS ==========

    // POST api/ClinicalOrder/master
    [HttpPost("master")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateMaster(
        [FromBody] CreateClinicalMasterDto dto)
    {
        var result = await _service.CreateMasterAsync(dto);
        return CreatedAtAction(nameof(GetMasterById),
            new { id = result.Clinicalmasterid }, result);
    }

    // GET api/ClinicalOrder/master
    [HttpGet("master")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetAllMasters()
    {
        var result = await _service.GetAllMastersAsync();
        return Ok(result);
    }

    // GET api/ClinicalOrder/master/5
    [HttpGet("master/{id}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetMasterById(long id)
    {
        var result = await _service.GetMasterByIdAsync(id);
        if (result == null)
            return NotFound(new
            {
                error = $"Clinical master with ID {id} not found."
            });
        return Ok(result);
    }

    // GET api/ClinicalOrder/master/type/Lab
    [HttpGet("master/type/{ordertype}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetMastersByType(string ordertype)
    {
        var validTypes = new[] { "Lab", "Imaging", "Medication" };
        if (!validTypes.Contains(ordertype, StringComparer.OrdinalIgnoreCase))
            return BadRequest(new
            {
                error = "Invalid order type.",
                allowed = validTypes
            });

        var result = await _service.GetMastersByTypeAsync(ordertype);
        return Ok(result);
    }

    // GET api/ClinicalOrder/master/search?keyword=cbc
    [HttpGet("master/search")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> SearchMasters([FromQuery] string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return BadRequest(new { error = "Keyword cannot be empty." });

        var result = await _service.SearchMastersAsync(keyword);
        return Ok(result);
    }

    // PUT api/ClinicalOrder/master/5
    [HttpPut("master/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateMaster(
        long id, [FromBody] UpdateClinicalMasterDto dto)
    {
        var result = await _service.UpdateMasterAsync(id, dto);
        if (result == null)
            return NotFound(new
            {
                error = $"Clinical master with ID {id} not found."
            });
        return Ok(result);
    }

    // DELETE api/ClinicalOrder/master/5
    [HttpDelete("master/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteMaster(long id)
    {
        var success = await _service.DeleteMasterAsync(id);
        if (!success)
            return NotFound(new
            {
                error = $"Clinical master with ID {id} not found."
            });
        return NoContent();
    }

    // ========== ORDER ENDPOINTS ==========

    // POST api/ClinicalOrder/orders
    [HttpPost("orders")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> CreateOrder(
        [FromBody] CreateClinicalOrderDto dto)
    {
        var result = await _service.CreateOrderAsync(dto);
        return CreatedAtAction(nameof(GetOrderById),
            new { id = result.Clinicalorderid }, result);
    }

    // GET api/ClinicalOrder/orders
    [HttpGet("orders")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllOrders()
    {
        var result = await _service.GetAllOrdersAsync();
        return Ok(result);
    }

    // GET api/ClinicalOrder/orders/5
    [HttpGet("orders/{id}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetOrderById(long id)
    {
        var result = await _service.GetOrderByIdAsync(id);
        if (result == null)
            return NotFound(new
            {
                error = $"Clinical order with ID {id} not found."
            });
        return Ok(result);
    }

    // GET api/ClinicalOrder/orders/patient/3
    [HttpGet("orders/patient/{patientId}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetOrdersByPatientId(long patientId)
    {
        var result = await _service.GetOrdersByPatientIdAsync(patientId);
        return Ok(result);
    }

    // GET api/ClinicalOrder/orders/provider/1
    [HttpGet("orders/provider/{providerInfoId}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetOrdersByProviderId(long providerInfoId)
    {
        var result = await _service.GetOrdersByProviderIdAsync(providerInfoId);
        return Ok(result);
    }

    // GET api/ClinicalOrder/orders/encounter/1
    [HttpGet("orders/encounter/{encounterId}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetOrdersByEncounterId(int encounterId)
    {
        var result = await _service.GetOrdersByEncounterIdAsync(encounterId);
        return Ok(result);
    }

    // GET api/ClinicalOrder/orders/status/Pending
    [HttpGet("orders/status/{status}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetOrdersByStatus(string status)
    {
        var validStatuses = new[]
        {
            "Pending", "InProgress", "Completed", "Cancelled"
        };
        if (!validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
            return BadRequest(new
            {
                error = "Invalid status.",
                allowed = validStatuses
            });

        var result = await _service.GetOrdersByStatusAsync(status);
        return Ok(result);
    }

    // GET api/ClinicalOrder/orders/type/Lab
    [HttpGet("orders/type/{ordertype}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetOrdersByType(string ordertype)
    {
        var validTypes = new[] { "Lab", "Imaging", "Medication" };
        if (!validTypes.Contains(ordertype, StringComparer.OrdinalIgnoreCase))
            return BadRequest(new
            {
                error = "Invalid order type.",
                allowed = validTypes
            });

        var result = await _service.GetOrdersByTypeAsync(ordertype);
        return Ok(result);
    }

    // GET api/ClinicalOrder/orders/urgent
    [HttpGet("orders/urgent")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetUrgentOrders()
    {
        var result = await _service.GetUrgentOrdersAsync();
        return Ok(result);
    }

    // PUT api/ClinicalOrder/orders/5
    [HttpPut("orders/{id}")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> UpdateOrder(
        long id, [FromBody] UpdateClinicalOrderDto dto)
    {
        var result = await _service.UpdateOrderAsync(id, dto);
        if (result == null)
            return NotFound(new
            {
                error = $"Clinical order with ID {id} not found."
            });
        return Ok(result);
    }

    // PATCH api/ClinicalOrder/orders/5/status
    [HttpPatch("orders/{id}/status")]
    [Authorize(Roles = "Provider,Admin")]
    public async Task<IActionResult> UpdateOrderStatus(
        long id, [FromBody] ClinicalOrderStatusUpdateDto dto)
    {
        var result = await _service.UpdateOrderStatusAsync(id, dto);
        if (result == null)
            return NotFound(new
            {
                error = $"Clinical order with ID {id} not found."
            });
        return Ok(result);
    }

    // DELETE api/ClinicalOrder/orders/5
    [HttpDelete("orders/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteOrder(long id)
    {
        var success = await _service.DeleteOrderAsync(id);
        if (!success)
            return NotFound(new
            {
                error = $"Clinical order with ID {id} not found."
            });
        return NoContent();
    }
    // POST api/ClinicalOrder/{orderId}/upload-file
    [HttpPost("{orderId}/upload-file")]
    [Authorize(Roles = "Provider,Admin")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadFileForOrder(long orderId, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { error = "File not provided." });

        var result = await _service.UploadOrderFileAsync(orderId, file);
        return Ok(result);
    }

}