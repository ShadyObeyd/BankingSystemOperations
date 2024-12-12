using System.ComponentModel.DataAnnotations;
using BankingSystemOperations.Data;
using BankingSystemOperations.Data.Dtos;
using BankingSystemOperations.Data.Mappers;
using BankingSystemOperations.Data.Validators;
using BankingSystemOperations.Services.Contracts;
using BankingSystemOperations.Services.Results;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemOperations.Services;

public class PartnersService : IPartnersService
{
    private readonly BankingOperationsContext _context;
    private readonly ICsvService _csvService;

    public PartnersService(BankingOperationsContext context, ICsvService csvService)
    {
        _context = context;
        _csvService = csvService;
    }
    
    public async Task<PaginatedList<PartnerDto>> GetPartnersAsync(int pageNumber, int pageSize)
    {
        int count = await _context.Partners.CountAsync();

        if (count == 0)
        {
            return new PaginatedList<PartnerDto>
            {
                Items = Enumerable.Empty<PartnerDto>(),
                TotalCount = 0,
                TotalPages = 0
            };
        }

        var partners = await _context.Partners
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(p => PartnersMapper.ToDto(p))
            .ToListAsync();

        return new PaginatedList<PartnerDto>
        {
            Items = partners,
            TotalCount = count,
            TotalPages = (int)Math.Ceiling((double)count / pageSize)
        };
    }

    public async Task<PartnerDto> GetPartnerByIdAsync(Guid partnerId)
    {
        var partner = await _context.Partners.FindAsync(partnerId);

        if (partner is null)
        {
            return null;
        }
        
        return PartnersMapper.ToDto(partner);
    }

    public async Task<PaginatedList<MerchantDto>> GetPartnerMerchantsByIdAsync(Guid partnerId, int pageNumber, int pageSize)
    {
        var partner = await _context.Partners
            .Include(p => p.Merchants)
            .FirstOrDefaultAsync(p => p.Id == partnerId);

        if (partner is null)
        {
            return new PaginatedList<MerchantDto>
            {
                Items = Enumerable.Empty<MerchantDto>(),
                TotalCount = 0,
                TotalPages = 0
            };
        }

        var query = partner.Merchants.AsQueryable();
        
        var count = await query.CountAsync();

        if (count == 0)
        {
            return new PaginatedList<MerchantDto>
            {
                Items = Enumerable.Empty<MerchantDto>(),
                TotalCount = 0,
                TotalPages = 0
            };
        }

        var merchants = await query
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

    public async Task<string> PreparePartnersForCsvExportAsync()
    {
        var partners = await _context.Partners.ToListAsync();
        
        var csvFormat = _csvService.PrepareCsvExport(partners);

        return csvFormat;
    }

    public async Task<ValidationResult> InsertPartnerAsync(PartnerDto dto)
    {
        if (dto is null)
        {
            return new ValidationResult("Invalid partner");
        }

        var validator = new PartnersValidator();
        var validationResult = await validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return new ValidationResult(string.Join(Environment.NewLine, errors));
        }

        var partner = PartnersMapper.ToEntity(dto);
        
        await _context.Partners.AddAsync(partner);
        await _context.SaveChangesAsync();
        
        return ValidationResult.Success;
    }

    public async Task<ValidationResult> UpdatePartnerAsync(PartnerDto dto)
    {
        if (dto is null)
        {
            return new ValidationResult("Invalid input");
        }
        
        var validator = new PartnersValidator();
        var validationResult = await validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => x.ErrorMessage);
            return new ValidationResult(string.Join(Environment.NewLine, errors));
        }
        
        if (!dto.Id.HasValue)
        {
            return new ValidationResult("Invalid partner id");
        }
        
        var partner = await _context.Partners.FindAsync(dto.Id.Value);

        if (partner is null)
        {
            return new ValidationResult("Partner not found");
        }
        
        partner.Name = dto.Name;
        
        _context.Partners.Update(partner);
        await _context.SaveChangesAsync();

        return ValidationResult.Success;
    }

    public async Task<ValidationResult> DeletePartnerAsync(Guid partnerId)
    {
        var partner = await _context.Partners
            .Include(p => p.Merchants)
            .FirstOrDefaultAsync(p => p.Id == partnerId);

        if (partner is null)
        {
            return new ValidationResult("Partner not found");
        }
        
        _context.Merchants.RemoveRange(partner.Merchants);
        _context.Partners.Remove(partner);
        
        await _context.SaveChangesAsync();
        
        return ValidationResult.Success;
    }
}