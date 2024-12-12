using System.Text;
using BankingSystemOperations.Data.Dtos;
using BankingSystemOperations.Services.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
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
    
    [HttpGet("ExportCsv")]
    public async Task<IActionResult> ExportCsv()
    {
        var csvData = await _merchantsService.PrepareMerchantsForCsvExportAsync();

        if (string.IsNullOrEmpty(csvData))
        {
            return NotFound("No merchants to export");
        }
        
        var fileBytes = Encoding.UTF8.GetBytes(csvData);

        return File(fileBytes, "text/csv", "Merchants.csv");
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateMerchant([FromBody] MerchantDto merchantDto)
    {
        if (merchantDto is null)
        {
            return BadRequest("Invalid input");
        }

        var result = await _merchantsService.InserMerchantAsync(merchantDto);

        if (!string.IsNullOrEmpty(result?.ErrorMessage))
        {
            return BadRequest(result.ErrorMessage);
        }

        return Created();
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateMerchant([FromBody] MerchantDto merchantDto)
    {
        if (merchantDto is null)
        {
            return BadRequest("Invalid input");
        }

        if (!merchantDto.Id.HasValue)
        {
            return BadRequest("Ivalid merchant id");
        }
        
        var result = await _merchantsService.UpdateMerchantAsync(merchantDto);

        if (!string.IsNullOrEmpty(result?.ErrorMessage))
        {
            return BadRequest(result.ErrorMessage);
        }

        return Ok();
    }
}