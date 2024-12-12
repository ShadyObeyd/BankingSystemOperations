using System.ComponentModel.DataAnnotations;
using BankingSystemOperations.Data;
using BankingSystemOperations.Data.Dtos;
using BankingSystemOperations.Data.Mappers;
using BankingSystemOperations.Services.Contracts;
using BankingSystemOperations.Services.Results;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemOperations.Services;

public class MerchantsService : IMerchantsService
{
    private readonly BankingOperationsContext _context;

    public MerchantsService(BankingOperationsContext context)
    {
        _context = context;
    }
    
    public async Task<PaginatedList<MerchantDto>> GetMerchantsAsync(int pageNumber, int pageSize)
    {
        int count = await _context.Merchants.CountAsync();

        if (count == 0)
        {
            return new PaginatedList<MerchantDto>
            {
                Items = Enumerable.Empty<MerchantDto>(),
                TotalCount = 0,
                TotalPages = 0
            };
        }

        var merchants = await _context.Merchants
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(m => MerchantsMapper.ToDto(m))
            .ToListAsync();

        return new PaginatedList<MerchantDto>
        {
            Items = merchants,
            TotalCount = count,
            TotalPages = (int)Math.Ceiling((double)count / pageSize)
        };
    }

    public async Task<MerchantDto> GetMerchantByIdAsync(Guid merchantId)
    {
        var merchant = await _context.Merchants.FindAsync(merchantId);

        if (merchant is null)
        {
            return null;
        }

        return MerchantsMapper.ToDto(merchant);
    }

    public async Task<string> PrepareMerchantsForCsvExportAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ValidationResult> InserMerchantAsync(MerchantDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<ValidationResult> UpdateMerchantAsync(MerchantDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<ValidationResult> DeleteMerchantAsync(Guid merchantId)
    {
        throw new NotImplementedException();
    }
}