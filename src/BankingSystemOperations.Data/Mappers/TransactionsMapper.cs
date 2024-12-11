using BankingSystemOperations.Data.Dtos;
using BankingSystemOperations.Data.Entities;

namespace BankingSystemOperations.Data.Mappers;

public class TransactionsMapper
{
    public static Transaction ToEntity(TransactionDto dto)
    {
        return new Transaction
        {
            CreateDate = dto.CreateDate,
            Direction = dto.Direction,
            Amount = dto.Amount,
            Currency = dto.Currency,
            DeptorIBAN = dto.DeptorIBAN,
            BeneficiaryIBAN = dto.BeneficiaryIBAN,
            Status = dto.Status,
            ExternalId = dto.ExternalId
        };
    }

    public static TransactionDto ToDto(Transaction transaction)
    {
        return new TransactionDto
        {
            Id = transaction.Id,
            CreateDate = transaction.CreateDate,
            Direction = transaction.Direction,
            Amount = transaction.Amount,
            Currency = transaction.Currency,
            DeptorIBAN = transaction.DeptorIBAN,
            BeneficiaryIBAN = transaction.BeneficiaryIBAN,
            Status = transaction.Status,
            ExternalId = transaction.ExternalId,
            MerchantId = transaction.MerchantId
        };
    }
}