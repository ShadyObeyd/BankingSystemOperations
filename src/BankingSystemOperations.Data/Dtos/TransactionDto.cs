using BankingSystemOperations.Data.Entities.Enums;

namespace BankingSystemOperations.Data.Dtos;

public class TransactionDto
{
    public Guid? Id { get; set; }
    
    public DateTime CreateDate { get; set; }
    
    public char Direction { get; set; }
    
    public decimal Amount { get; set; }
    
    public string Currency { get; set; }
    
    public string DeptorIBAN { get; set; }
    
    public string BeneficiaryIBAN { get; set; }
    
    public TransactionStatus? Status { get; set; }
    
    public string ExternalId { get; set; }
    
    public Guid? MerchantId { get; set; }
}