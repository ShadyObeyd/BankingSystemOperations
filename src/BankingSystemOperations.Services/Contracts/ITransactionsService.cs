using System.ComponentModel.DataAnnotations;
using BankingSystemOperations.Data.Dtos;
using Microsoft.AspNetCore.Http;

namespace BankingSystemOperations.Services.Contracts;

public interface ITransactionsService
{
    Task<ValidationResult> InsertTransactionsAsync(IEnumerable<TransactionDto> transactionDtos);
    
    IEnumerable<TransactionDto?> ReadXML(IFormFile file);

    Task<string> ExportTransactionsToCsvAsync();
}