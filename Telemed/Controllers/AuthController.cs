using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs.Auth;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    // ---------------- PATIENT ----------------
    [HttpPost("register/Patient")]
    public async Task<IActionResult> RegisterPatient([FromBody] RegisterPatientDto dto)
    {
        return Ok(await _service.RegisterPatientAsync(dto));
    }

    // ---------------- PROVIDER ----------------
    [HttpPost("register/Provider")]
    public async Task<IActionResult> RegisterProvider([FromBody] RegisterProviderDto dto)
    {
        return Ok(await _service.RegisterProviderAsync(dto));
    }

    // ---------------- ADMIN ----------------
    [HttpPost("register/Admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminDto dto)
    {
        return Ok(await _service.RegisterAdminAsync(dto));
    }

    // ---------------- LOGIN (all roles) ----------------
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        return Ok(await _service.LoginAsync(dto));
    }

    // ---------------- REFRESH TOKEN ----------------
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
    {
        return Ok(await _service.RefreshTokenAsync(dto));
    }

    // ---------------- LOGOUT ----------------
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (claim == null) return Unauthorized();

        await _service.LogoutAsync(int.Parse(claim.Value));
        return Ok(new { message = "Logout successful" });
    }

    [Authorize]
    [HttpGet("debug-auth")]
    public IActionResult DebugAuth()
    {
        return Ok(new
        {
            IsAuthenticated = User.Identity?.IsAuthenticated,
            UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
            Email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value,
            Role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value,
            ReferenceId = User.FindFirst("ReferenceId")?.Value,
            Fullname = User.FindFirst("Fullname")?.Value
        });
    }
}