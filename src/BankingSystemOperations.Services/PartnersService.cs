using System.ComponentModel.DataAnnotations;
using BankingSystemOperations.Data;
using BankingSystemOperations.Data.Dtos;
using BankingSystemOperations.Data.Mappers;
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

    public async Task<string> PreparePartnersForCsvExportAsync()
    {
        var partners = await _context.Partners.ToListAsync();
        
        var csvFormat = _csvService.PrepareCsvExport(partners);

        return csvFormat;
    }

    public Task<ValidationResult> InsertPartnerAsync(PartnerDto partner)
    {
        throw new NotImplementedException();
    }
}