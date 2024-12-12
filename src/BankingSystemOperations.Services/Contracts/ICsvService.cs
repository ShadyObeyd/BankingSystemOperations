namespace BankingSystemOperations.Services.Contracts;

public interface ICsvService
{
    string PrepareCsvExport<BaseEntity>(IEnumerable<BaseEntity> entities);
}