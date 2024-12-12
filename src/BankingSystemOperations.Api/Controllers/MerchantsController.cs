using System.ComponentModel.DataAnnotations;
using System.Text;
using BankingSystemOperations.Data.Dtos;
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
    
    [HttpGet("{id}/Partner")]
    public async Task<IActionResult> GetMerchantPartner(Guid id)
    {
        var partner = await _merchantsService.GetMerchantPartnerByIdAsync(id);

        if (partner is null)
        {
            return NotFound("Partner not found");
        }
        
        return Ok(partner);
    }
    
    [HttpGet("{id}/Transactions")]
    public async Task<IActionResult> GetMerchantTransactions(Guid id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var transactions = await _merchantsService.GetMerchantTranscationsByIdAsync(id, pageNumber, pageSize);

        if (transactions.TotalCount == 0)
        {
            return NotFound("No transactions found");
        }

        return Ok(transactions);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateMerchant([FromBody] MerchantDto merchantDto)
    {
        if (merchantDto is null)
        {
            return BadRequest("Invalid input");
        }

        var result = await _merchantsService.InserMerchantAsync(merchantDto);

        if (result != ValidationResult.Success)
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

        if (result != ValidationResult.Success)
        {
            return BadRequest(result.ErrorMessage);
        }

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMerchant(Guid id)
    {
        var result = await _merchantsService.DeleteMerchantAsync(id);

        if (result != ValidationResult.Success)
        {
            return BadRequest(result.ErrorMessage);
        }

        return Ok();
    }
}