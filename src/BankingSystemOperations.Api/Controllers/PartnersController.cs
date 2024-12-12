using BankingSystemOperations.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystemOperations.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PartnersController : ControllerBase
{
    private readonly IPartnersService _partnersService;

    public PartnersController(IPartnersService partnersService)
    {
        _partnersService = partnersService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPartners(int page = 1, int pageSize = 10)
    {
        var partners = await _partnersService.GetPartnersAsync(page, pageSize);

        if (partners.TotalCount == 0)
        {
            return NotFound("No partners found");
        }
        
        return Ok(partners);
    }
}