using System.ComponentModel.DataAnnotations;
using BankingSystemOperations.Data.Dtos;
using BankingSystemOperations.Services.Filters;
using BankingSystemOperations.Services.Results;
using Microsoft.AspNetCore.Http;

namespace BankingSystemOperations.Services.Contracts;

public interface ITransactionsService
{
    Task<ValidationResult> InsertTransactionsAsync(IEnumerable<TransactionDto> transactionDtos);
    
    IEnumerable<TransactionDto> ReadXML(IFormFile file);

    Task<string> PrepareTransactionsForCsvExportAsync();
    
    Task<TransactionDto> GetTransactionByIdAsync(Guid transactionId);
    
    Task<MerchantDto> GetTransactionMerchantByIdAsync(Guid transactionId);
    
    Task<PaginatedList<TransactionDto>> GetTransactionsAsync(int pageNumber, int pageSize, TransactionFilter filter);
}