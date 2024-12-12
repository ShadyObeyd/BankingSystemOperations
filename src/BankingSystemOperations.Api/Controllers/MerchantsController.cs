using BankingSystemOperations.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystemOperations.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MerchantsController : ControllerBase
{
    private readonly IMerchantsService _merchantsService;

    public MerchantsController(IMerchantsService merchantsService)
    {
        _merchantsService = merchantsService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetMerchants([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var merchants = await _merchantsService.GetMerchantsAsync(pageNumber, pageSize);

        if (merchants.TotalCount == 0)
        {
            return NotFound("No merchants found");
        }
        
        return Ok(merchants);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMerchant(Guid id)
    {
        var merchant = await _merchantsService.GetMerchantByIdAsync(id);

        if (merchant is null)
        {
            return NotFound("Merchant not found");
        }
        
        return Ok(merchant);
    }
}