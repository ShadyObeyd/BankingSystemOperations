using System.ComponentModel.DataAnnotations;
using BankingSystemOperations.Data;
using BankingSystemOperations.Data.Dtos;
using BankingSystemOperations.Data.Mappers;
using BankingSystemOperations.Data.Validators;
using BankingSystemOperations.Services.Contracts;
using BankingSystemOperations.Services.Results;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemOperations.Services;

public class MerchantsService : IMerchantsService
{
    private readonly BankingOperationsContext _context;
    private readonly ICsvService _csvService;

    public MerchantsService(BankingOperationsContext context, ICsvService csvService)
    {
        _context = context;
        _csvService = csvService;
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
        var merchants = await _context.Merchants.ToListAsync();
        
        var csvFormat = _csvService.PrepareCsvExport(merchants);
        
        return csvFormat;
    }

    public async Task<ValidationResult> InserMerchantAsync(MerchantDto dto)
    {
        if (dto is null)
        {
            return new ValidationResult("Invalid merchant");
        }

        var validator = new MerchantsValidator();

        var validationResult = await validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => x.ErrorMessage);
            return new ValidationResult(string.Join(Environment.NewLine, errors));
        }
        
        var merchant = MerchantsMapper.ToEntity(dto);

        if (dto.PartnerId.HasValue)
        {
            var partner = await _context.Partners.FirstOrDefaultAsync(p => p.Id == dto.PartnerId);

            if (partner is not null)
            {
                merchant.PartnerId = partner.Id;
                merchant.Partner = partner;
            }
        }
        
        await _context.Merchants.AddAsync(merchant);
        await _context.SaveChangesAsync();
        
        return ValidationResult.Success;
    }

    public async Task<ValidationResult> UpdateMerchantAsync(MerchantDto dto)
    {
        if (dto is null)
        {
            return new ValidationResult("Invalid input");
        }

        var validator = new MerchantsValidator();

        var validationResult = await validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => x.ErrorMessage);
            return new ValidationResult(string.Join(Environment.NewLine, errors));
        }

        if (!dto.Id.HasValue)
        {
            return new ValidationResult("Invalid merchant id");
        }
        
        var merchant = await _context.Merchants.FindAsync(dto.Id.Value);

        if (merchant is null)
        {
            return new ValidationResult("Merchant not found");
        }
        
        merchant.Name = dto.Name;
        merchant.BoardingDate = dto.BoardingDate;
        merchant.Url = dto.Url;
        merchant.Country = dto.Country;
        merchant.FirstAddress = dto.FirstAddress;
        merchant.SecondAddress = dto.SecondAddress;

        _context.Update(merchant);
        await _context.SaveChangesAsync();
        
        return ValidationResult.Success;
    }

    public async Task<ValidationResult> DeleteMerchantAsync(Guid merchantId)
    {
        var merchant = await _context.Merchants.FindAsync(merchantId);

        if (merchant is null)
        {
            return new ValidationResult("Merchant not found");
        }
        
        var transactions = await _context.Transactions
            .Where(t => t.MerchantId == merchantId)
            .ToListAsync();

        _context.Transactions.RemoveRange(transactions);
        _context.Merchants.Remove(merchant);
        
        await _context.SaveChangesAsync();

        return ValidationResult.Success;
    }
}