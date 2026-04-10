using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[Route("api/providerinfo")]
[ApiController]
public class ProviderInfoController : ControllerBase
{
    private readonly IProviderInfoService _service;

    public ProviderInfoController(IProviderInfoService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ProviderInfoResponseDto>> Create([FromForm] CreateProviderInfoDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Providerinfoid }, result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProviderInfoResponseDto>>> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<ProviderInfoResponseDto>> GetById(long id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<ProviderInfoResponseDto>> GetByEmail(string email)
    {
        var result = await _service.GetByEmailAsync(email);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet("speciality/{speciality}")]
    public async Task<ActionResult<IEnumerable<ProviderInfoResponseDto>>> GetBySpeciality(string speciality)
        => Ok(await _service.GetBySpecialityAsync(speciality));

    [HttpGet("state/{state}")]
    public async Task<ActionResult<IEnumerable<ProviderInfoResponseDto>>> GetByState(string state)
        => Ok(await _service.GetByStateAsync(state));

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<ProviderInfoResponseDto>>> GetActive()
        => Ok(await _service.GetActiveAsync());

    [HttpPut("{id}")]
    public async Task<ActionResult<ProviderInfoResponseDto>> Update(long id, [FromForm] UpdateProviderInfoDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPut("{id}/picture")]
    public async Task<ActionResult<ProviderProfilePictureResponseDto>> UpdateProfilePicture(
        long id, IFormFile picture, [FromQuery] long? updatedby)
    {
        var result = await _service.UpdateProfilePictureAsync(id, picture, updatedby);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet("{id}/picture")]
    public async Task<IActionResult> GetProfilePicture(long id)
    {
        var picture = await _service.GetProfilePictureAsync(id);
        return picture == null ? NotFound() : File(picture, "image/jpeg");
    }

    [HttpPut("{id}/deactivate")]
    public async Task<IActionResult> Deactivate(long id, [FromQuery] long? updatedby)
    {
        var success = await _service.DeactivateAsync(id, updatedby);
        return success ? Ok(new { message = "Provider deactivated successfully" }) : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var success = await _service.DeleteAsync(id);
        return success ? Ok(new { message = "Provider deleted successfully" }) : NotFound();
    }
}