using BankingSystemOperations.Data.Dtos;
using BankingSystemOperations.Data.Entities;

namespace BankingSystemOperations.Data.Mappers;

public class PartnersMapper
{
    public static PartnerDto ToDto(Partner partner)
    {
        return new PartnerDto
        {
            Id = partner.Id,
            Name = partner.Name
        };
    }

    public static Partner ToEntity(PartnerDto partnerDto)
    {
        return new Partner
        {
            Name = partnerDto.Name
        };
    }
}