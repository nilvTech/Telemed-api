// Mappers/ProviderInfoMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class ProviderInfoMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    public static Providerinfo ToProviderinfoEntity(
        CreateProviderInfoDto dto,
        byte[]? profilePictureBytes = null)
    {
        return new Providerinfo
        {
            Firstname = dto.Firstname,
            Lastname = dto.Lastname,
            Email = dto.Email,
            Phone = dto.Phone,
            Gender = dto.Gender,
            Role = "Provider",
            Displayname = dto.Displayname ??
                             $"Dr. {dto.Firstname} {dto.Lastname}".Trim(),
            Profilepicture = profilePictureBytes,
            Createdby = dto.Createdby,
            Createdat = ToUnspecified(DateTime.UtcNow),
            Updatedat = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static Providerprofile ToProviderprofileEntity(
        CreateProviderInfoDto dto,
        long providerinfoid)
    {
        return new Providerprofile
        {
            Providerinfoid = providerinfoid,
            Providertype = dto.Providertype,
            Bio = dto.Bio,
            Yearofexperience = dto.Yearofexperience,
            Licensenumber = dto.Licensenumber,
            NpiNumber = dto.NpiNumber,
            Secondaryrole = dto.Secondaryrole,
            Speciality1 = dto.Speciality1,
            Speciality2 = dto.Speciality2,
            Website = dto.Website,
            Timezone = dto.Timezone,
            Addressline1 = dto.Addressline1,
            Addressline2 = dto.Addressline2,
            City = dto.City,
            State = dto.State,
            Zip = dto.Zip,
            Country = dto.Country ?? "United States",
            Isactive = true,
            Createdby = dto.Createdby,
            Createdat = ToUnspecified(DateTime.UtcNow),
            Updatedat = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateProviderinfoEntity(
        Providerinfo entity,
        UpdateProviderInfoDto dto,
        byte[]? profilePictureBytes = null)
    {
        if (!string.IsNullOrEmpty(dto.Firstname))
            entity.Firstname = dto.Firstname;

        if (!string.IsNullOrEmpty(dto.Lastname))
            entity.Lastname = dto.Lastname;

        if (!string.IsNullOrEmpty(dto.Phone))
            entity.Phone = dto.Phone;

        if (!string.IsNullOrEmpty(dto.Gender))
            entity.Gender = dto.Gender;

        if (!string.IsNullOrEmpty(dto.Displayname))
            entity.Displayname = dto.Displayname;

        if (profilePictureBytes != null)
            entity.Profilepicture = profilePictureBytes;

        entity.Updatedby = dto.Updatedby;
        entity.Updatedat = ToUnspecified(DateTime.UtcNow);
    }

    public static void UpdateProviderprofileEntity(
        Providerprofile entity,
        UpdateProviderInfoDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Providertype))
            entity.Providertype = dto.Providertype;

        if (!string.IsNullOrEmpty(dto.Bio))
            entity.Bio = dto.Bio;

        if (dto.Yearofexperience.HasValue)
            entity.Yearofexperience = dto.Yearofexperience;

        if (!string.IsNullOrEmpty(dto.Licensenumber))
            entity.Licensenumber = dto.Licensenumber;

        if (!string.IsNullOrEmpty(dto.NpiNumber))
            entity.NpiNumber = dto.NpiNumber;

        if (!string.IsNullOrEmpty(dto.Secondaryrole))
            entity.Secondaryrole = dto.Secondaryrole;

        if (!string.IsNullOrEmpty(dto.Speciality1))
            entity.Speciality1 = dto.Speciality1;

        if (!string.IsNullOrEmpty(dto.Speciality2))
            entity.Speciality2 = dto.Speciality2;

        if (!string.IsNullOrEmpty(dto.Website))
            entity.Website = dto.Website;

        if (!string.IsNullOrEmpty(dto.Timezone))
            entity.Timezone = dto.Timezone;

        if (!string.IsNullOrEmpty(dto.Addressline1))
            entity.Addressline1 = dto.Addressline1;

        if (!string.IsNullOrEmpty(dto.Addressline2))
            entity.Addressline2 = dto.Addressline2;

        if (!string.IsNullOrEmpty(dto.City))
            entity.City = dto.City;

        if (!string.IsNullOrEmpty(dto.State))
            entity.State = dto.State;

        if (!string.IsNullOrEmpty(dto.Zip))
            entity.Zip = dto.Zip;

        if (!string.IsNullOrEmpty(dto.Country))
            entity.Country = dto.Country;

        if (dto.Isactive.HasValue)
            entity.Isactive = dto.Isactive;

        entity.Updatedby = dto.Updatedby;
        entity.Updatedat = ToUnspecified(DateTime.UtcNow);
    }

    public static ProviderInfoResponseDto ToResponseDto(Providerinfo entity)
    {
        var profile = entity.Providerprofile;

        // Build full address
        string? fullAddress = null;
        if (profile != null)
        {
            var parts = new List<string>();
            if (!string.IsNullOrEmpty(profile.Addressline1))
                parts.Add(profile.Addressline1);
            if (!string.IsNullOrEmpty(profile.Addressline2))
                parts.Add(profile.Addressline2);
            if (!string.IsNullOrEmpty(profile.City))
                parts.Add(profile.City);
            if (!string.IsNullOrEmpty(profile.State))
                parts.Add(profile.State);
            if (!string.IsNullOrEmpty(profile.Zip))
                parts.Add(profile.Zip);
            if (!string.IsNullOrEmpty(profile.Country))
                parts.Add(profile.Country);
            fullAddress = string.Join(", ", parts);
        }

        // Convert profile picture to Base64 for frontend display
        string? pictureBase64 = null;
        if (entity.Profilepicture != null && entity.Profilepicture.Length > 0)
            pictureBase64 = Convert.ToBase64String(entity.Profilepicture);

        return new ProviderInfoResponseDto
        {
            // Providerinfo
            Providerinfoid = entity.Providerinfoid,
            Firstname = entity.Firstname,
            Lastname = entity.Lastname,
            Fullname = $"{entity.Firstname} {entity.Lastname}".Trim(),
            Displayname = entity.Displayname,
            Email = entity.Email,
            Phone = entity.Phone,
            Gender = entity.Gender,
            Role = entity.Role,
            HasProfilePicture = entity.Profilepicture != null &&
                                    entity.Profilepicture.Length > 0,
            ProfilepictureBase64 = pictureBase64,
            Createdat = entity.Createdat,
            Updatedat = entity.Updatedat,

            // Providerprofile
            Profileid = profile?.Profileid,
            Providertype = profile?.Providertype,
            Bio = profile?.Bio,
            Yearofexperience = profile?.Yearofexperience,
            Licensenumber = profile?.Licensenumber,
            NpiNumber = profile?.NpiNumber,
            Secondaryrole = profile?.Secondaryrole,
            Speciality1 = profile?.Speciality1,
            Speciality2 = profile?.Speciality2,
            Website = profile?.Website,
            Timezone = profile?.Timezone,
            Addressline1 = profile?.Addressline1,
            Addressline2 = profile?.Addressline2,
            City = profile?.City,
            State = profile?.State,
            Zip = profile?.Zip,
            Country = profile?.Country,
            Isactive = profile?.Isactive,
            Fulladdress = fullAddress
        };
    }
}