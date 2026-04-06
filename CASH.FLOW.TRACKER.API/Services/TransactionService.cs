using BUGET.TRACKER.API.Model;
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
                TransactionDate = addTransactionDTO.TransactionDate
            };

            var result = await _transactionRepository.AddTransactionAsync(transaction, ct);

            if(result is null)
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

        public async Task<GetTransactionDTO?> GetTransactionByIdAsync(Guid transactionId, CancellationToken ct)
        {
            var payload = await _transactionRepository.GetTransactionByIdAsync(transactionId, ct);

            if (payload is null)
                throw new TransactionNotFoundException();

            return payload;
        }

        public async Task DeleteTransactionAsync(Guid transactionId, CancellationToken ct)
        {
            var isDeleted = await _transactionRepository.DeleteTransactionAsync(transactionId, ct);

            if (!isDeleted)
                throw new NoTransactionExistingException();
        }

        public async Task<GetTransactionDTO> UpdateTransactionAsync(UpdateTransactionDTO updateTransactionDTO , CancellationToken ct)
        {
            var transaction = await _transactionRepository.UpdateTransactionAsync(updateTransactionDTO, ct);

            if (transaction is null)
                throw new TransactionNotFoundException();

            return transaction;
        }
    }
}
