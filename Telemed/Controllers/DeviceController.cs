// Controllers/DeviceController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DeviceController : ControllerBase
{
    private readonly IDeviceService _service;

    public DeviceController(IDeviceService service)
    {
        _service = service;
    }

    // POST api/Device
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm] CreateDeviceDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { id = result.Id }, result);
    }

    // GET api/Device
    [HttpGet]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    // GET api/Device/5
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null)
            return NotFound(new { error = $"Device with ID {id} not found." });
        return Ok(result);
    }

    // GET api/Device/deviceid/DEV-001
    [HttpGet("deviceid/{deviceId}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByDeviceId(string deviceId)
    {
        var result = await _service.GetByDeviceIdAsync(deviceId);
        if (result == null)
            return NotFound(new
            {
                error = $"Device with device ID '{deviceId}' not found."
            });
        return Ok(result);
    }

    // GET api/Device/serial/SN-12345
    [HttpGet("serial/{serialNumber}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetBySerialNumber(string serialNumber)
    {
        var result = await _service.GetBySerialNumberAsync(serialNumber);
        if (result == null)
            return NotFound(new
            {
                error = $"Device with serial number '{serialNumber}' not found."
            });
        return Ok(result);
    }

    // GET api/Device/manufacturer/Philips
    [HttpGet("manufacturer/{manufacturer}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByManufacturer(string manufacturer)
    {
        var result = await _service.GetByManufacturerAsync(manufacturer);
        return Ok(result);
    }

    // GET api/Device/active
    [HttpGet("active")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetActive()
    {
        var result = await _service.GetActiveAsync();
        return Ok(result);
    }

    // PUT api/Device/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Update(
        long id, [FromForm] UpdateDeviceDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new { error = $"Device with ID {id} not found." });
        return Ok(result);
    }

    // PATCH api/Device/5/picture
    [HttpPatch("{id}/picture")]
    [Authorize(Roles = "Admin")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdatePicture(
        long id, IFormFile picture)
    {
        var result = await _service.UpdatePictureAsync(id, picture);
        if (result == null)
            return NotFound(new { error = $"Device with ID {id} not found." });
        return Ok(result);
    }

    // GET api/Device/5/picture
    [HttpGet("{id}/picture")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetDevicePicture(long id)
    {
        var pictureBytes = await _service.GetDevicePictureAsync(id);
        if (pictureBytes == null || pictureBytes.Length == 0)
            return NotFound(new { error = "No picture found for this device." });

        return File(pictureBytes, "image/jpeg");
    }

    // PATCH api/Device/5/toggle-status
    [HttpPatch("{id}/toggle-status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ToggleStatus(long id)
    {
        var success = await _service.ToggleStatusAsync(id);
        if (!success)
            return NotFound(new { error = $"Device with ID {id} not found." });
        return Ok(new { message = "Device status updated successfully." });
    }

    // DELETE api/Device/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(long id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(new { error = $"Device with ID {id} not found." });
        return NoContent();
    }
}