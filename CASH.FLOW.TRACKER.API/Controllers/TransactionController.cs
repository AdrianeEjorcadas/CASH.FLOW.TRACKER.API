using CASH.FLOW.TRACKER.API.Helpers.Pagination.Parameters;
using CASH.FLOW.TRACKER.API.Model.DTO.Transactions;
using CASH.FLOW.TRACKER.API.Model.Response;
using CASH.FLOW.TRACKER.API.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CASH.FLOW.TRACKER.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        
        [HttpPost("transaction")]
        public async Task<ActionResult<ReturnResponse<object>>> AddTransactionAsync([FromBody] AddTransactionDTO addTransactionDTO, 
            CancellationToken ct = default) 
        {
            await _transactionService.AddTransactionAsync(addTransactionDTO, ct);

            return Ok(new ReturnResponse<object>
            {
                StatusCode = 200,
                Message = "Successfully added",
                Data = null
            });
        }

        [HttpGet("transactions")]
        public async Task<ActionResult<ReturnResponse<IEnumerable<GetTransactionDTO>>>> GetTransactionsAsync([FromQuery] Guid userId,
            CancellationToken ct = default)
        {
            var payload = await _transactionService.GetTransactionsAsync(userId, ct);

            return Ok(new ReturnResponse<IEnumerable<GetTransactionDTO>>
            {
                StatusCode = 200,
                Message = "Successfully retrieve",
                Data = payload
            });
        }

        [HttpGet("transaction-by-id")]
        public async Task<ActionResult<ReturnResponse<GetTransactionDTO>>> GetTransactionByIdAsync([FromQuery]GetTransactionByIdDTO transactionByIdDTO, 
            CancellationToken ct = default)
        {
            var payload = await _transactionService.GetTransactionByIdAsync(transactionByIdDTO, ct);

            return Ok(new ReturnResponse<GetTransactionDTO>
            {
                StatusCode = 200,
                Message = "Successfully retrieve",
                Data = payload
            });
        }

        [HttpDelete("delete-transaction")]
        public async Task<ActionResult<ReturnResponse<object>>> DeleteTransactionAsync([FromQuery] DeleteTransactionDTO deleteTransactionDTO,
            CancellationToken ct = default)
        {
            await _transactionService.DeleteTransactionAsync(deleteTransactionDTO, ct);

            return Ok(new ReturnResponse<object>
            {
                StatusCode = 200,
                Message = "Successfully deleted",
                Data = null
            });
        }

        [HttpPatch("update-transaction")]
        public async Task<ActionResult<ReturnResponse<GetTransactionDTO>>> UpdateTransactionAsync([FromBody] UpdateTransactionDTO updateTransactionDTO, 
            CancellationToken ct = default)
        {
            var payload = await _transactionService.UpdateTransactionAsync(updateTransactionDTO, ct);

            return Ok(new ReturnResponse<GetTransactionDTO>
            {
                StatusCode = 200,
                Message = "Updated Successfully",
                Data = payload
            });
        }

        [HttpGet("get-transactions")]
        public async Task<ActionResult<ReturnResponse<GetTransactionPagedDTO>>> GetTransactionsPagedAsync([FromQuery]TransactionParameters transactionParameters, CancellationToken ct = default)
        {
            var result = await _transactionService.GetTransactionsPagedAsync(transactionParameters, ct);

            Response.Headers["X-Transactions-Pagination"] = JsonSerializer.Serialize(result.metadata);

            return Ok(new ReturnResponse<GetTransactionPagedDTO>
            {
                StatusCode = 200,
                Message = "Successfully retrieve transactions",
                Data = new GetTransactionPagedDTO
                {
                    Transactions = result.transactions,
                    Metadata = result.metadata
                }
            });
        }
    }
}