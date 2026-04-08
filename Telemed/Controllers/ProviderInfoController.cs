// Controllers/ProviderInfoController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProviderInfoController : ControllerBase
{
    private readonly IProviderInfoService _service;

    public ProviderInfoController(IProviderInfoService service)
    {
        _service = service;
    }

    // POST api/ProviderInfo
    // Creates Providerinfo + Providerprofile together
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create(
        [FromForm] CreateProviderInfoDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { id = result.Providerinfoid }, result);
    }

    // GET api/ProviderInfo
    [HttpGet]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    // GET api/ProviderInfo/5
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null)
            return NotFound(new { error = $"Provider with ID {id} not found." });
        return Ok(result);
    }

    // GET api/ProviderInfo/email/dr.smith@telemed.com
    [HttpGet("email/{email}")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var result = await _service.GetByEmailAsync(email);
        if (result == null)
            return NotFound(new { error = $"Provider with email '{email}' not found." });
        return Ok(result);
    }

    // GET api/ProviderInfo/speciality/Cardiology
    [HttpGet("speciality/{speciality}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetBySpeciality(string speciality)
    {
        var result = await _service.GetBySpecialityAsync(speciality);
        return Ok(result);
    }

    // GET api/ProviderInfo/state/CA
    // Important for US telemedicine license verification
    [HttpGet("state/{state}")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetByState(string state)
    {
        var result = await _service.GetByStateAsync(state);
        return Ok(result);
    }

    // GET api/ProviderInfo/active
    [HttpGet("active")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetActive()
    {
        var result = await _service.GetActiveAsync();
        return Ok(result);
    }

    // PUT api/ProviderInfo/5
    // Updates Providerinfo + Providerprofile together
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Provider")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Update(
        long id, [FromForm] UpdateProviderInfoDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new { error = $"Provider with ID {id} not found." });
        return Ok(result);
    }

    // PATCH api/ProviderInfo/5/picture
    // Update profile picture only
    [HttpPatch("{id}/picture")]
    [Authorize(Roles = "Admin,Provider")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateProfilePicture(
        long id,
        IFormFile picture,
        [FromQuery] long? updatedby)
    {
        var result = await _service
            .UpdateProfilePictureAsync(id, picture, updatedby);
        if (result == null)
            return NotFound(new { error = $"Provider with ID {id} not found." });
        return Ok(result);
    }

    // GET api/ProviderInfo/5/picture
    // Get profile picture as image file
    [HttpGet("{id}/picture")]
    [Authorize(Roles = "Admin,Provider,Patient")]
    public async Task<IActionResult> GetProfilePicture(long id)
    {
        var pictureBytes = await _service.GetProfilePictureAsync(id);
        if (pictureBytes == null || pictureBytes.Length == 0)
            return NotFound(new { error = "No profile picture found." });

        // Return as image
        return File(pictureBytes, "image/jpeg");
    }

    // PATCH api/ProviderInfo/5/deactivate
    [HttpPatch("{id}/deactivate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Deactivate(
        long id, [FromQuery] long? updatedby)
    {
        var success = await _service.DeactivateAsync(id, updatedby);
        if (!success)
            return NotFound(new { error = $"Provider with ID {id} not found." });
        return Ok(new { message = $"Provider {id} deactivated successfully." });
    }

    // DELETE api/ProviderInfo/5
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