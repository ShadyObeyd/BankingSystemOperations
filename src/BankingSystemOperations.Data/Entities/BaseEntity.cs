using System.ComponentModel.DataAnnotations;

namespace BankingSystemOperations.Data.Entities;

public abstract class BaseEntity
{
    protected const int MinLength = 2;
    protected const int MaxLength = 100;
    
    public BaseEntity()
    {
        Id = Guid.NewGuid();
    }
    
    [Key]
    public Guid Id { get; set; }
}