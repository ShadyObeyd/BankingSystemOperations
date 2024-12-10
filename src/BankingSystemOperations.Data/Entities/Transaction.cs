using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystemOperations.Data.Entities;

public class Transaction : BaseEntity
{
    public DateTime CreateDate { get; set; }
    
    [Required]
    [MinLength(1)]
    [MaxLength(1)]
    public char Direction { get; set; }
    
    [Range(typeof(decimal),"0", "79228162514264337593543950335")]
    public decimal Amount { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(3)]
    public string? Currency { get; set; }
    
    [Required]
    [MinLength(15)]
    [MaxLength(34)]
    public string DeptorIBAN { get; set; }
    
    [Required]
    [MinLength(15)]
    [MaxLength(34)]
    public string BeneficiaryIBAN { get; set; }
    
    [Range(0, 1)]
    public byte Status { get; set; }
    
    [Required]
    [MinLength(MinLength)]
    [MaxLength(MaxLength)]
    public string ExternalId { get; set; }
    
    public Guid? MerchantId { get; set; }
    
    [ForeignKey(nameof(MerchantId))]
    public Merchant? Merchant { get; set; }
}