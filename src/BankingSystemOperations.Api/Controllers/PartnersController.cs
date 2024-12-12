using System.Text;
using BankingSystemOperations.Data.Dtos;
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
    public async Task<IActionResult> GetPartners([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var partners = await _partnersService.GetPartnersAsync(page, pageSize);

        if (partners.TotalCount == 0)
        {
            return NotFound("No partners found");
        }
        
        return Ok(partners);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPartnerById(Guid id)
    {
        var partner = await _partnersService.GetPartnerByIdAsync(id);

        if (partner is null)
        {
            return NotFound("Partner not found");
        }
        
        return Ok(partner);
    }
    
    [HttpGet("ExportCsv")]
    public async Task<IActionResult> ExportCsv()
    {
        var csvData = await _partnersService.PreparePartnersForCsvExportAsync();

        if (string.IsNullOrEmpty(csvData))
        {
            return NotFound("No partners to export");
        }
        
        var fileBytes = Encoding.UTF8.GetBytes(csvData);

        return File(fileBytes, "text/csv", "Partners.csv");
    }
    
    [HttpPost]
    public async Task<IActionResult> CreatePartner([FromBody] PartnerDto dto)
    {
        var result = await _partnersService.InsertPartnerAsync(dto);

        if (result?.ErrorMessage is not null)
        {
            return BadRequest(result.ErrorMessage);
        }
        
        return Created();
    }
}