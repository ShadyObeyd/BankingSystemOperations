namespace BankingSystemOperations.Services.Results;

public class PaginatedList<T>
{
    public IEnumerable<T> Items { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }
}