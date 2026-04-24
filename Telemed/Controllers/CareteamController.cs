// Controllers/CareteamController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CareteamController : ControllerBase
{
    private readonly ICareteamService _service;

    public CareteamController(ICareteamService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateCareteamDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { id = result.Careteamid }, result);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Provider,Patient")]
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
            return NotFound(new
            {
                error = $"Care team with ID {id} not found."
            });
        return Ok(result);
    }

    // GET api/Careteam/search?keyword=cardiac
    [HttpGet("search")]
    [Authorize(Roles = "Admin,Provider")]
    public async Task<IActionResult> Search([FromQuery] string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return BadRequest(new { error = "Keyword cannot be empty." });

        var result = await _service.SearchAsync(keyword);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        long id, [FromBody] UpdateCareteamDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(new
            {
                error = $"Care team with ID {id} not found."
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
                error = $"Care team with ID {id} not found."
            });
        return NoContent();
    }
}