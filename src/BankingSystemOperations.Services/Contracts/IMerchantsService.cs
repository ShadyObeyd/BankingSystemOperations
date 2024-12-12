using System.ComponentModel.DataAnnotations;
using BankingSystemOperations.Data.Dtos;
using BankingSystemOperations.Services.Results;

namespace BankingSystemOperations.Services.Contracts;

public interface IMerchantsService
{
    Task<PaginatedList<MerchantDto>> GetMerchantsAsync(int pageNumber, int pageSize);
    
    Task<MerchantDto> GetMerchantByIdAsync(Guid merchantId);

    Task<string> PrepareMerchantsForCsvExportAsync();
    
    Task<ValidationResult> InserMerchantAsync(MerchantDto dto);
    
    Task<ValidationResult> UpdateMerchantAsync(MerchantDto dto);
    
    Task<ValidationResult> DeleteMerchantAsync(Guid merchantId);
}