using System.Text;
using BankingSystemOperations.Services.Contracts;
using BankingSystemOperations.Services.Filters;
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
    public async Task<IActionResult> GetTransactions([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] TransactionFilter filter = null)
    {
        if (pageNumber <= 0 || pageSize <= 0)
        {
            return BadRequest("Invalid page number or page size");
        }
        
        var transactions = await _transactionsService.GetTransactionsAsync(pageNumber, pageSize, filter);

        if (transactions.Items.Count() == 0)
        {
            return NotFound("No transactions found");
        }

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
        var csvData = await _transactionsService.PrepareTransactionsForCsvExportAsync();

        if (string.IsNullOrEmpty(csvData))
        {
            return NotFound("No transactions to export");
        }
        
        var fileBytes = Encoding.UTF8.GetBytes(csvData);
        
        return File(fileBytes, "text/csv", "Transactions.csv");
    }
    
    [HttpGet("{id}/Merchant")]
    public async Task<IActionResult> GetTransactionMerchant(Guid id)
    {
        var merchant = await _transactionsService.GetTransactionMerchantByIdAsync(id);

        if (merchant is null)
        {
            return NotFound("No merchant found");
        }
        
        return Ok(merchant);
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
        
        return Created();
    }
}