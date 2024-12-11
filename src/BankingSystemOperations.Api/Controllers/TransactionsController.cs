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
    
    [HttpPost]
    public async Task<IActionResult> ImportXML(IFormFile? file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Invalid file");
        }

        var dtos = _transactionsService.ReadXML(file);

        var result = await _transactionsService.InsertTransactions(dtos);

        if (!string.IsNullOrEmpty(result?.ErrorMessage))
        {
            return BadRequest(result.ErrorMessage);
        }
        
        return Ok();
    }
}