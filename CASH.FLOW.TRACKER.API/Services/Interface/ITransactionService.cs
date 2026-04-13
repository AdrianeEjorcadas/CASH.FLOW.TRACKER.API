using CASH.FLOW.TRACKER.API.Helpers.Pagination;
using CASH.FLOW.TRACKER.API.Helpers.Pagination.Parameters;
using CASH.FLOW.TRACKER.API.Model.DTO.Transactions;

namespace CASH.FLOW.TRACKER.API.Services.Interface
{
    public interface ITransactionService
    {
        Task<bool> AddTransactionAsync(AddTransactionDTO addTransactionDTO, CancellationToken ct);
        Task DeleteTransactionAsync(DeleteTransactionDTO deleteTransactionDTO, CancellationToken ct);
        Task<GetTransactionDTO?> GetTransactionByIdAsync(GetTransactionByIdDTO transactionByIdDTO, CancellationToken ct);
        Task<IEnumerable<GetTransactionDTO>> GetTransactionsAsync(Guid userId, CancellationToken ct);
        Task<(IEnumerable<GetTransactionDTO> transactions, Metadata metadata)> GetTransactionsPagedAsync(TransactionParameters transactionParameters, CancellationToken ct);
        Task<GetTransactionDTO> UpdateTransactionAsync(UpdateTransactionDTO updateTransactionDTO, CancellationToken ct);
    }
}
