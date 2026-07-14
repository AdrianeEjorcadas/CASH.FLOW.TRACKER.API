using BUGET.TRACKER.API.Model;
using CASH.FLOW.TRACKER.API.Helpers.Pagination;
using CASH.FLOW.TRACKER.API.Helpers.Pagination.Parameters;
using CASH.FLOW.TRACKER.API.Middleware.Exceptions;
using CASH.FLOW.TRACKER.API.Model.DTO.Transactions;
using CASH.FLOW.TRACKER.API.Repositories.Interface;
using CASH.FLOW.TRACKER.API.Services.Interface;

namespace CASH.FLOW.TRACKER.API.Services
{
    public class TransactionService : ITransactionService
    {

        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository  = transactionRepository;
        }

        public async Task<bool> AddTransactionAsync(AddTransactionDTO addTransactionDTO, CancellationToken ct)
        {
            var transaction = new Transaction
            {
                UserId = addTransactionDTO.UserId,
                TransactionName = addTransactionDTO.TransactionName,
                CategoryId = addTransactionDTO.CategoryId,
                Amount = addTransactionDTO.Amount,
                Note = addTransactionDTO.Note,
                //TransactionDate = addTransactionDTO.TransactionDate
                // Take only the calendar date the client intended, drop time-of-day/offset entirely.
                // This guarantees .Year/.Month reads back identical to what was written.
                TransactionDate = new DateTimeOffset(addTransactionDTO.TransactionDate.Date, TimeSpan.Zero),
            };

            var result = await _transactionRepository.AddTransactionAsync(transaction, ct);

            if (result is null)
                throw new TransactionException(addTransactionDTO.TransactionName);

            return true;
        }

        public async Task<IEnumerable<GetTransactionDTO>> GetTransactionsAsync(Guid userId, CancellationToken ct)
        {
            var paylaoad = await _transactionRepository.GetTransactionsAsync(userId, ct);

            if (!paylaoad.Any())
                throw new TransactionNotFoundException();

            return paylaoad;
        }

        public async Task<GetTransactionDTO?> GetTransactionByIdAsync(GetTransactionByIdDTO transactionByIdDTO, CancellationToken ct)
        {
            var payload = await _transactionRepository.GetTransactionByIdAsync(transactionByIdDTO, ct);

            if (payload is null)
                throw new TransactionNotFoundException();

            return payload;
        }

        public async Task DeleteTransactionAsync(DeleteTransactionDTO deleteTransactionDTO, CancellationToken ct)
        {
            var isDeleted = await _transactionRepository.DeleteTransactionAsync(deleteTransactionDTO, ct);

            if (!isDeleted)
                throw new NoTransactionExistingException();
        }

        public async Task UpdateTransactionAsync(UpdateTransactionDTO updateTransactionDTO , CancellationToken ct)
        {
            var isUpdated = await _transactionRepository.UpdateTransactionAsync(updateTransactionDTO, ct);

            if (!isUpdated)
                throw new TransactionNotFoundException();

        }

        public async Task<(IEnumerable<GetTransactionDTO> transactions, Metadata metadata)> GetTransactionsPagedAsync(TransactionParameters transactionParameters, CancellationToken ct)
        {
            var result = await _transactionRepository.GetTransactionsPagedAsync(transactionParameters, ct);

            if (result.Metadata.TotalCount <= 0)
                throw new NoTransactionExistingException();

            return (transactions: result, metadata: result.Metadata);
        }
    }
}
