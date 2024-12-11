using System.Text;
using BankingSystemOperations.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystemOperations.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionsService _transactionsService;

    public TransactionsController(ITransactionsService transactionsService)
    {
        _transactionsService = transactionsService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTransactions([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        if (pageNumber <= 0 || pageSize <= 0)
        {
            return BadRequest("Invalid page number or page size");
        }
        
        var transactions = await _transactionsService.GetTransactionsAsync(pageNumber, pageSize);

        return Ok(transactions);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTransactionById(Guid id)
    {
        var transaction = await _transactionsService.GetTransactionByIdAsync(id);

        if (transaction is null)
        {
            return NotFound("No transaction found");
        }

        return Ok(transaction);
    }
    
    [HttpGet("ExportCsv")]
    public async Task<IActionResult> ExportCsv()
    {
        var csvData = await _transactionsService.ExportTransactionsToCsvAsync();

        if (string.IsNullOrEmpty(csvData))
        {
            return NotFound("No transactions to export");
        }
        
        var fileBytes = Encoding.UTF8.GetBytes(csvData);
        
        return File(fileBytes, "text/csv", "Transactions.csv");
    }
    
    [HttpPost("ImportXml")]
    public async Task<IActionResult> ImportXML(IFormFile? file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Invalid file");
        }
    
        var dtos = _transactionsService.ReadXML(file);

        var result = await _transactionsService.InsertTransactionsAsync(dtos);

        if (!string.IsNullOrEmpty(result?.ErrorMessage))
        {
            return BadRequest(result.ErrorMessage);
        }
        
        return Ok();
    }
}