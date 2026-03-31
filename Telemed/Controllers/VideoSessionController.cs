using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VideoSessionController : ControllerBase
    {
        private readonly IVideoSessionService _service;

        public VideoSessionController(IVideoSessionService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Provider")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Provider,Patient")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> Create([FromBody] CreateVideoSessionDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Provider")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateVideoSessionDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? Ok() : NotFound();
        }
    }
}