using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Xml.Linq;
using BankingSystemOperations.Data;
using BankingSystemOperations.Data.Dtos;
using BankingSystemOperations.Data.Entities;
using BankingSystemOperations.Data.Entities.Enums;
using BankingSystemOperations.Data.Mappers;
using BankingSystemOperations.Data.Validators;
using BankingSystemOperations.Services.Contracts;
using BankingSystemOperations.Services.Filters;
using BankingSystemOperations.Services.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemOperations.Services;

public class TransactionsService : ITransactionsService
{
    private readonly BankingOperationsContext _context;
    private readonly ICsvService _csvService;

    public TransactionsService(BankingOperationsContext context, ICsvService csvService)
    {
        _context = context;
        _csvService = csvService;
    }

    public async Task<ValidationResult> InsertTransactionsAsync(IEnumerable<TransactionDto> transactionDtos)
    {
        var validator = new TransactionsValidator();
        
        foreach (var dto in transactionDtos)
        {
            var existingTransaction = await _context.Transactions.FirstOrDefaultAsync(x => x.ExternalId == dto.ExternalId);

            if (existingTransaction is not null)
            {
                return new ValidationResult($"Transaction with external Id: {dto.ExternalId} already exists.");
            }
            
            var result = validator.Validate(dto);

            if (!result.IsValid)
            {
                StringBuilder sb = new($"Errors for transaction with external Id: {dto.ExternalId}");

                foreach (var error in result.Errors)
                {
                    sb.AppendLine(error.ErrorMessage);
                }
                
                return new ValidationResult(sb.ToString().Trim());
            }

            var transaction = TransactionsMapper.ToEntity(dto);

            if (dto.MerchantId.HasValue)
            {
                var merchant = await _context.Merchants.FirstOrDefaultAsync(x => x.Id == dto.MerchantId);

                if (merchant is not null)
                {
                    transaction.MerchantId = merchant.Id;
                    transaction.Merchant = merchant;
                }
            }
            
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        return ValidationResult.Success;
    }

    public IEnumerable<TransactionDto> ReadXML(IFormFile file)
    {
        using var stream = file.OpenReadStream();
        var xmlDoc = XDocument.Load(stream);

        var transactions = xmlDoc.Descendants("Transaction").Select(t => new TransactionDto
        {
            CreateDate = DateTime.ParseExact(t.Element("CreateDate").Value, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal),
            Direction = char.TryParse(t.Element("Amount").Element("Direction").Value, out char direction) ? direction : ' ',
            Amount = decimal.TryParse(t.Element("Amount").Element("Value").Value, CultureInfo.InvariantCulture, out decimal amount) ? amount : 0,
            Currency = t.Element("Amount")?.Element("Currency").Value,
            DeptorIBAN = t.Element("Debtor")?.Element("IBAN")?.Value,
            BeneficiaryIBAN = t.Element("Beneficiary")?.Element("IBAN")?.Value ?? string.Empty,
            Status = Enum.TryParse(t.Element("Status").Value, out TransactionStatus status) ? status : null,
            ExternalId = t.Element("ExternalId")?.Value,
        }).ToList();

        return transactions;
    }

    public async Task<string> PrepareTransactionsForCsvExportAsync()
    {
        var transactions = await _context.Transactions.ToListAsync();
        
        var csvFormat = _csvService.PrepareCsvExport(transactions);

        return csvFormat;
    }

    public async Task<TransactionDto> GetTransactionByIdAsync(Guid transactionId)
    {
        var transaction = await _context.Transactions.FindAsync(transactionId);

        if (transaction is null)
        {
            return null;
        }

        return TransactionsMapper.ToDto(transaction);
    }

    public async Task<PaginatedList<TransactionDto>> GetTransactionsAsync(int pageNumber, int pageSize, TransactionFilter filter)
    {
        IQueryable<Transaction> query;

        if (filter is null)
        {
            query = _context.Transactions.AsQueryable();
        }
        else
        {
            query = GetFilteredTransactionsAsync(filter);
        }

        int count = await query.CountAsync();

        if (count == 0)
        {
            return new PaginatedList<TransactionDto>
            {
                Items = Enumerable.Empty<TransactionDto>(),
                TotalPages = 0,
                TotalCount = 0
            };
        }
        
        var transactions = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(t => TransactionsMapper.ToDto(t))
            .ToListAsync();

        return new PaginatedList<TransactionDto>
        {
            TotalCount = count,
            TotalPages = (int)Math.Ceiling((double)count / pageSize),
            Items = transactions
        };
    }

    private IQueryable<Transaction> GetFilteredTransactionsAsync(TransactionFilter filter)
    {
        var query = _context.Transactions.AsQueryable();

        if (filter.CreateDateFrom.HasValue)
            query = query.Where(t => t.CreateDate >= filter.CreateDateFrom.Value);

        if (filter.CreateDateTo.HasValue)
            query = query.Where(t => t.CreateDate <= filter.CreateDateTo.Value);

        if (filter.Direction.HasValue)
            query = query.Where(t => t.Direction.ToString().ToLower() == filter.Direction.Value.ToString().ToLower());

        if (filter.AmountMin.HasValue)
            query = query.Where(t => t.Amount >= filter.AmountMin.Value);

        if (filter.AmountMax.HasValue)
            query = query.Where(t => t.Amount <= filter.AmountMax.Value);

        if (!string.IsNullOrEmpty(filter.Currency))
            query = query.Where(t => t.Currency.ToLower() == filter.Currency.ToLower());

        if (!string.IsNullOrEmpty(filter.DeptorIBAN))
            query = query.Where(t => t.DeptorIBAN == filter.DeptorIBAN);

        if (!string.IsNullOrEmpty(filter.BeneficiaryIBAN))
            query = query.Where(t => t.BeneficiaryIBAN == filter.BeneficiaryIBAN);

        if (filter.Status.HasValue)
            query = query.Where(t => t.Status == filter.Status.Value);

        return query;
    }
}