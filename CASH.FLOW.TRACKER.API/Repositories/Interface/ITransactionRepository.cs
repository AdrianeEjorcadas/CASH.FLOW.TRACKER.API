using BUGET.TRACKER.API.Model;
using CASH.FLOW.TRACKER.API.Helpers.Pagination;
using CASH.FLOW.TRACKER.API.Helpers.Pagination.Parameters;
using CASH.FLOW.TRACKER.API.Model.DTO.Transactions;

namespace CASH.FLOW.TRACKER.API.Repositories.Interface
{
    public interface ITransactionRepository
    {
        Task<Transaction> AddTransactionAsync(Transaction transaction, CancellationToken ct);
        Task<bool> DeleteTransactionAsync(DeleteTransactionDTO deleteTransactionDTO, CancellationToken ct);
        Task<GetTransactionDTO?> GetTransactionByIdAsync(GetTransactionByIdDTO transactionByIdDTO, CancellationToken ct);
        Task<IEnumerable<GetTransactionDTO>> GetTransactionsAsync(Guid userId, CancellationToken ct);
        Task<PagedList<GetTransactionDTO>> GetTransactionsPagedAsync(TransactionParameters transactionParameters, CancellationToken ct);
        Task<bool> UpdateTransactionAsync(UpdateTransactionDTO updateTransactionDTO, CancellationToken ct);
        //Task<(GetTransactionDTO? transaction, bool isSuccess)> UpdateTransactionAsync(UpdateTransactionDTO updateTransactionDTO, CancellationToken ct);
    }
}
