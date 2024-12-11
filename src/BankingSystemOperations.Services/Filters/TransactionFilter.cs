using BankingSystemOperations.Data.Entities.Enums;

namespace BankingSystemOperations.Services.Filters;

public class TransactionFilter
{
    public DateTime? CreateDateFrom { get; set; }
    
    public DateTime? CreateDateTo { get; set; }
    
    public char? Direction { get; set; }
    
    public decimal? AmountMin { get; set; }
    
    public decimal? AmountMax { get; set; }
    
    public string? Currency { get; set; }
    
    public string? DeptorIBAN { get; set; }
    
    public string? BeneficiaryIBAN { get; set; }
    
    public TransactionStatus? Status { get; set; }
}