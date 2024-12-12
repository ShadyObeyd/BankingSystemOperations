using BankingSystemOperations.Data.Dtos;
using BankingSystemOperations.Data.Entities;

namespace BankingSystemOperations.Data.Mappers;

public class MerchantsMapper
{
    public static MerchantDto ToDto(Merchant merchant)
    {
        return new MerchantDto
        {
            Id = merchant.Id,
            Name = merchant.Name,
            BoardingDate = merchant.BoardingDate,
            Url = merchant.Url,
            Country = merchant.Country,
            FirstAddress = merchant.FirstAddress,
            SecondAddress = merchant.SecondAddress,
            PartnerId = merchant.PartnerId
        };
    }

    public static Merchant ToEntity(MerchantDto merchantDto)
    {
        return new Merchant
        {
            Name = merchantDto.Name,
            BoardingDate = merchantDto.BoardingDate,
            Url = merchantDto.Url,
            Country = merchantDto.Country,
            FirstAddress = merchantDto.FirstAddress,
            SecondAddress = merchantDto.SecondAddress,
            PartnerId = merchantDto.PartnerId
        };
    }
}