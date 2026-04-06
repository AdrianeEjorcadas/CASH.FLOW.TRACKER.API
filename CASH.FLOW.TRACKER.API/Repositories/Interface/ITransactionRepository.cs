using BUGET.TRACKER.API.Model;
using CASH.FLOW.TRACKER.API.Model.DTO.Transactions;

namespace CASH.FLOW.TRACKER.API.Repositories.Interface
{
    public interface ITransactionRepository
    {
        Task<Transaction> AddTransactionAsync(Transaction transaction, CancellationToken ct);
        Task<bool> DeleteTransactionAsync(Guid transactionId, CancellationToken ct);
        Task<GetTransactionDTO?> GetTransactionByIdAsync(Guid transactionId, CancellationToken ct);
        Task<IEnumerable<GetTransactionDTO>> GetTransactionsAsync(Guid userId, CancellationToken ct);
        Task<GetTransactionDTO?> UpdateTransactionAsync(UpdateTransactionDTO updateTransactionDTO, CancellationToken ct);
        //Task<(GetTransactionDTO? transaction, bool isSuccess)> UpdateTransactionAsync(UpdateTransactionDTO updateTransactionDTO, CancellationToken ct);
    }
}
