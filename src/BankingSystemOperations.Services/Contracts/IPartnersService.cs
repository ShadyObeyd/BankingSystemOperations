using System.ComponentModel.DataAnnotations;
using BankingSystemOperations.Data.Dtos;
using BankingSystemOperations.Services.Results;

namespace BankingSystemOperations.Services.Contracts;

public interface IPartnersService
{
    Task<PaginatedList<PartnerDto>> GetPartnersAsync(int pageNumber, int pageSize);
    
    Task<PartnerDto> GetPartnerByIdAsync(Guid id);

    Task<string> PreparePartnersForCsvExportAsync();
    
    Task<ValidationResult> InsertPartnerAsync(PartnerDto partner);
}