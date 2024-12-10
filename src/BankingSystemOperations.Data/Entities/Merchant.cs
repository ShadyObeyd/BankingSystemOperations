using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystemOperations.Data.Entities;

public class Merchant : BaseEntity
{
    public Merchant()
    {
        Transactions = new List<Transaction>();
    }
    
    [Required]
    [MinLength(MinLength)]
    [MaxLength(MaxLength)]
    public string Name { get; set; }
    
    public DateTime BoardingDate { get; set; }
    
    [RegularExpression("^(https?:\\/\\/)?([\\w\\-]+(\\.[\\w\\-]+)+)(:\\d+)?(\\/[\\w\\-.,@?^=%&:/~+#]*)?$")]
    [MaxLength(2048)]
    public string? Url { get; set; }
    
    [Required]
    [MinLength(MinLength)]
    [MaxLength(MaxLength)]
    public string Country { get; set; }
    
    [Required]
    [MinLength(MinLength)]
    [MaxLength(MaxLength)]
    public string FirstAddress { get; set; }
    
    [MinLength(MinLength)]
    [MaxLength(MaxLength)]
    public string? SecondAddress { get; set; }
    
    public Guid? PartnerId { get; set; }
    
    [ForeignKey(nameof(PartnerId))]
    public Partner? Partner { get; set; }

    public IList<Transaction> Transactions { get; set; }
}