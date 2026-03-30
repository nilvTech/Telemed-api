using Microsoft.EntityFrameworkCore;
using Telemed.DTOs.Auth;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class AuthService : IAuthService
{
    private readonly TelemedDbContext _context;
    private readonly JwtService _jwtService;
    private readonly IConfiguration _config;

    public AuthService(
        TelemedDbContext context,
        JwtService jwtService,
        IConfiguration config)
    {
        _context = context;
        _jwtService = jwtService;
        _config = config;
    }

    // -------------------------------------------------------
    // REGISTER PATIENT
    // -------------------------------------------------------
    public async Task<AuthResponseDto> RegisterPatientAsync(RegisterPatientDto dto)
    {
        if (await _context.Users.AnyAsync(x => x.Email == dto.Email))
            throw new ArgumentException("Email already registered.");

        var patient = new Patient
        {
            Firstname = dto.Firstname,
            Middlename = dto.Middlename,
            Lastname = dto.Lastname,
            Email = dto.Email,
            Phone = dto.Phone,
            Gender = dto.Gender,
            Dateofbirth = dto.Dateofbirth.HasValue
                ? DateOnly.FromDateTime(dto.Dateofbirth.Value)
                : null,
            Address = dto.Address
        };

        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        var refreshToken = _jwtService.GenerateRefreshToken();

        var user = new User
        {
            Email = dto.Email,
            Passwordhash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = "Patient",
            Referenceid = patient.Patientid,
            Isactive = true,
            Createdat = DateTime.UtcNow,
            Refreshtoken = refreshToken,
            Refreshtokenexpiry = DateTime.UtcNow.AddDays(
               double.Parse(_config["JwtSettings:RefreshTokenExpiryDays"]!))
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var fullname = $"{patient.Firstname} {patient.Lastname}".Trim();

        var token = _jwtService.GenerateToken(user, fullname);

        return BuildAuthResponse(user, token, refreshToken, fullname);
    }

    // -------------------------------------------------------
    // REGISTER PROVIDER
    // -------------------------------------------------------
    public async Task<AuthResponseDto> RegisterProviderAsync(RegisterProviderDto dto)
    {
        if (await _context.Users.AnyAsync(x => x.Email == dto.Email))
            throw new ArgumentException("Email already registered.");

        var provider = new Provider
        {
            Providername = dto.Providername,
            Email = dto.Email,
            Phone = dto.Phone,
            Speciality = dto.Speciality,
            Website = dto.Website,
            Primaryaddress = dto.Primaryaddress,
            Status = "Active",
            Createdat = DateTime.UtcNow
        };

        _context.Providers.Add(provider);
        await _context.SaveChangesAsync();

        var refreshToken = _jwtService.GenerateRefreshToken();

        var user = new User
        {
            Email = dto.Email,
            Passwordhash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = "Provider",
            Referenceid = provider.Providerid,
            Isactive = true,
            Createdat = DateTime.UtcNow,
            Refreshtoken = refreshToken,
            Refreshtokenexpiry = DateTime.UtcNow.AddDays(
                double.Parse(_config["JwtSettings:RefreshTokenExpiryDays"]!))
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = _jwtService.GenerateToken(user, provider.Providername);

        return BuildAuthResponse(user, token, refreshToken, provider.Providername);
    }

    // -------------------------------------------------------
    // REGISTER ADMIN (FIXED)
    // -------------------------------------------------------
    public async Task<AuthResponseDto> RegisterAdminAsync(RegisterAdminDto dto)
    {
        if (await _context.Users.AnyAsync(x => x.Email == dto.Email))
            throw new ArgumentException("Email already registered.");

        var admin = new Admin
        {
            Firstname = dto.AdminName,   // Store in Firstname
            Lastname = "",               // No lastname provided
            Email = dto.Email,
            Createdate = DateTime.UtcNow
        };

        _context.Admins.Add(admin);
        await _context.SaveChangesAsync();

        var refreshToken = _jwtService.GenerateRefreshToken();

        var user = new User
        {
            Email = dto.Email,
            Passwordhash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = "Admin",
            Referenceid = admin.Adminid,
            Isactive = true,
            Createdat = DateTime.UtcNow,
            Refreshtoken = refreshToken,
            Refreshtokenexpiry = DateTime.UtcNow.AddDays(
                double.Parse(_config["JwtSettings:RefreshTokenExpiryDays"]!))
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var fullname = $"{admin.Firstname} {admin.Lastname}".Trim();

        var token = _jwtService.GenerateToken(user, fullname);

        return BuildAuthResponse(user, token, refreshToken, fullname);
    }

    // -------------------------------------------------------
    // LOGIN
    // -------------------------------------------------------
    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
            throw new ArgumentException("Invalid email or password.");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Passwordhash))
            throw new ArgumentException("Invalid email or password.");

        if (!user.Isactive.GetValueOrDefault())
            throw new ArgumentException("Account is deactivated.");

        var refreshToken = _jwtService.GenerateRefreshToken();
        user.Refreshtoken = refreshToken;
        user.Refreshtokenexpiry = DateTime.UtcNow.AddDays(
            double.Parse(_config["JwtSettings:RefreshTokenExpiryDays"]!));
        user.Lastloginat = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var fullname = await ResolveFullnameAsync(user.Role, user.Referenceid);
        var token = _jwtService.GenerateToken(user, fullname);

        return BuildAuthResponse(user, token, refreshToken, fullname);
    }

    // -------------------------------------------------------
    // REFRESH TOKEN
    // -------------------------------------------------------
    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Refreshtoken == dto.RefreshToken);

        if (user == null)
            throw new ArgumentException("Invalid refresh token.");

        if (user.Refreshtokenexpiry < DateTime.UtcNow)
            throw new ArgumentException("Refresh token expired.");

        var newRefreshToken = _jwtService.GenerateRefreshToken();
        user.Refreshtoken = newRefreshToken;
        user.Refreshtokenexpiry = DateTime.UtcNow.AddDays(
            double.Parse(_config["JwtSettings:RefreshTokenExpiryDays"]!));

        await _context.SaveChangesAsync();

        var fullname = await ResolveFullnameAsync(user.Role, user.Referenceid);
        var token = _jwtService.GenerateToken(user, fullname);

        return BuildAuthResponse(user, token, newRefreshToken, fullname);
    }

    // -------------------------------------------------------
    // LOGOUT
    // -------------------------------------------------------
    public async Task<bool> LogoutAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return false;

        user.Refreshtoken = null;
        user.Refreshtokenexpiry = null;

        await _context.SaveChangesAsync();
        return true;
    }

    // -------------------------------------------------------
    // Build Auth Response
    // -------------------------------------------------------
    private AuthResponseDto BuildAuthResponse(User user, string token, string refreshToken, string fullname)
    {
        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            Role = user.Role,
            UserId = user.Userid,
            ReferenceId = user.Referenceid,
            Email = user.Email,
            Fullname = fullname,
            ExpiresAt = DateTime.UtcNow.AddMinutes(
                double.Parse(_config["JwtSettings:ExpiryMinutes"]!))
        };
    }

    // -------------------------------------------------------
    // Resolve Fullname
    // -------------------------------------------------------
    private async Task<string> ResolveFullnameAsync(string role, int referenceId)
    {
        if (role == "Patient")
        {
            var p = await _context.Patients.FirstOrDefaultAsync(x => x.Patientid == referenceId);
            return p != null ? $"{p.Firstname} {p.Lastname}".Trim() : "Unknown";
        }

        if (role == "Provider")
        {
            var pr = await _context.Providers.FirstOrDefaultAsync(x => x.Providerid == referenceId);
            return pr?.Providername ?? "Unknown";
        }

        if (role == "Admin")
        {
            var ad = await _context.Admins.FirstOrDefaultAsync(x => x.Adminid == referenceId);
            return ad != null ? $"{ad.Firstname} {ad.Lastname}".Trim() : "Admin";
        }

        return "Unknown";
    }
}