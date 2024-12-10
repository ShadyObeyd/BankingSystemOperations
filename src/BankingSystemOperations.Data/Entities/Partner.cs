using System.ComponentModel.DataAnnotations;

namespace BankingSystemOperations.Data.Entities;

public class Partner : BaseEntity
{
    public Partner()
    {
        Merchants = new List<Merchant>();
    }
    
    [Required]
    [MinLength(MinLength)]
    [MaxLength(MaxLength)]
    public string Name { get; set; }
    
    public IList<Merchant> Merchants { get; set; }
}