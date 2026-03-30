using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappings;

public static class ProviderMapper
{
    public static ProviderDto ToDto(this Provider p)
    {
        return new ProviderDto
        {
            ProviderId = p.Providerid,
            ProviderName = p.Providername,
            Email = p.Email,
            Phone = p.Phone,
            Speciality = p.Speciality,
            Website = p.Website,
            PrimaryAddress = p.Primaryaddress,
            Status = p.Status
        };
    }

    public static Provider ToEntity(this CreateProviderDto dto)
    {
        return new Provider
        {
            Providername = dto.ProviderName,
            Email = dto.Email,
            Phone = dto.Phone,
            Speciality = dto.Speciality,
            Website = dto.Website,
            Primaryaddress = dto.PrimaryAddress,
            Status = dto.Status
        };
    }
}