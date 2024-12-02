using Microsoft.AspNetCore.Mvc;
using POS_API.DTOs;
using POS_API.Services;

namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionService _transactionService;

        public TransactionController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        // POST: api/pos/transactions
        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionDTO transactionDto)
        {
            try
            {
                //validation
                if ((transactionDto.Quantity == null || transactionDto.Quantity == 0) ||
                   (transactionDto.ItemId == null))
                {
                    return BadRequest("Bad Request, Check your inputs. ItemId and Quantity cannot be 0");
                }
                var result = await _transactionService.CreateTransactionAsync(transactionDto);

                if (result.Contains("not found"))
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/pos/transactions
        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {
            var transactions = await _transactionService.GetAllTransctionAsync();
            return Ok(transactions);

        }
    }
}
