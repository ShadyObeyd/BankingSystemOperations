namespace BankingSystemOperations.Data.Dtos;

public class MerchantDto
{
    public Guid? Id { get; set; }

    public string Name { get; set; }
    
    public DateTime BoardingDate { get; set; }
    
    public string? Url { get; set; }
    
    public string Country { get; set; }
    
    public string FirstAddress { get; set; }
    
    public string? SecondAddress { get; set; }
    
    public Guid? PartnerId { get; set; }
}