// DTOs/CreateProviderInfoDto.cs
namespace Telemed.DTOs;

public class CreateProviderInfoDto
{
    // ===== Providerinfo Fields =====
    public string Firstname { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Gender { get; set; }
    public string? Displayname { get; set; }
    public long? Createdby { get; set; }

    // Profile picture uploaded as file
    public IFormFile? Profilepicture { get; set; }

    // ===== Providerprofile Fields =====
    public string Providertype { get; set; } = null!;
    public string? Bio { get; set; }
    public int? Yearofexperience { get; set; }
    public string? Licensenumber { get; set; }
    public string? NpiNumber { get; set; }
    public string? Secondaryrole { get; set; }
    public string Speciality1 { get; set; } = null!;
    public string? Speciality2 { get; set; }
    public string? Website { get; set; }
    public string Timezone { get; set; } = "America/New_York";
    public string Addressline1 { get; set; } = null!;
    public string? Addressline2 { get; set; }
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string Zip { get; set; } = null!;
    public string? Country { get; set; } = "United States";
}