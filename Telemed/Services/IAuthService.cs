// Services/Interfaces/IAuthService.cs
using Telemed.DTOs.Auth;

namespace Telemed.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterPatientAsync(RegisterPatientDto dto);
    Task<AuthResponseDto> RegisterProviderAsync(RegisterProviderDto dto);
    Task<AuthResponseDto> RegisterAdminAsync(RegisterAdminDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto);
    Task<bool> LogoutAsync(int userId);
}   