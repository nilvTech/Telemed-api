// DTOs/ProviderInfoResponseDto.cs
namespace Telemed.DTOs;

public class ProviderInfoResponseDto
{
    // ===== Providerinfo =====
    public long Providerinfoid { get; set; }
    public string? GroupName { get; set; }

    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Fullname { get; set; }
    public string? Displayname { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Gender { get; set; }
    public string? Role { get; set; }
    public bool HasProfilePicture { get; set; }
    public string? ProfilepictureBase64 { get; set; } // For display in frontend
    public DateTime? Createdat { get; set; }
    public DateTime? Updatedat { get; set; }

    // ===== Providerprofile =====
    public long? Profileid { get; set; }
    public string? Providertype { get; set; }
    public string? Bio { get; set; }
    public int? Yearofexperience { get; set; }
    public string? Licensenumber { get; set; }
    public string? NpiNumber { get; set; }
    public string? Secondaryrole { get; set; }
    public string? Speciality1 { get; set; }
    public string? Speciality2 { get; set; }
    public string? Website { get; set; }
    public string? Timezone { get; set; }
    public string? Addressline1 { get; set; }
    public string? Addressline2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public string? Country { get; set; }
    public bool? Isactive { get; set; }
    public string? Fulladdress { get; set; } // Auto formatted
}