using BUGET.TRACKER.API.Data;
using BUGET.TRACKER.API.Model;
using CASH.FLOW.TRACKER.API.Helpers.Pagination;
using CASH.FLOW.TRACKER.API.Helpers.Pagination.Parameters;
using CASH.FLOW.TRACKER.API.Model.DTO.Transactions;
using CASH.FLOW.TRACKER.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace CASH.FLOW.TRACKER.API.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {

        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction> AddTransactionAsync(Transaction transaction, CancellationToken ct)
        {
            var payload = await _context.Transactions
                .AddAsync(transaction, ct);

            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<IEnumerable<GetTransactionDTO>> GetTransactionsAsync(Guid userId, CancellationToken ct)
        {
            var payload = await _context.Transactions
                .AsNoTracking()
                .Where(t => t.UserId == userId)
                .Include(t => t.Category)
                .Select(t => new GetTransactionDTO
                {
                    TransactionId  = t.TransactionId,   
                    TransactionName = t.TransactionName,
                    Amount = t.Amount,
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category.CategoryName,
                    Note = t.Note,
                    TransactionDate = t.TransactionDate,
                    UserId = t.UserId
                })
                .ToListAsync(ct);

            return payload;
        }

        public async Task<GetTransactionDTO?> GetTransactionByIdAsync(GetTransactionByIdDTO transactionByIdDTO, CancellationToken ct)
        {
            var payload = await _context.Transactions
                .Where(t => t.TransactionId == transactionByIdDTO.TransactionID && t.UserId == transactionByIdDTO.UserId)
                .Select(t => new GetTransactionDTO
                {
                    TransactionId = t.TransactionId,
                    TransactionName = t.TransactionName,
                    Amount = t.Amount,
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category.CategoryName,
                    Note = t.Note,
                    TransactionDate = t.TransactionDate,
                    UserId = t.UserId
                })
                .FirstOrDefaultAsync(ct);

            return payload;
        }

        public async Task<bool> DeleteTransactionAsync(DeleteTransactionDTO deleteTransactionDTO, CancellationToken ct)
        {
            var transaction = await _context.Transactions
                .Where(t => t.TransactionId == deleteTransactionDTO.TransactionId && t.UserId == deleteTransactionDTO.UserId)
                .FirstOrDefaultAsync(ct);

            if (transaction is null)
                return false;

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync(ct);

            return true;
        }

        public async Task<GetTransactionDTO?> UpdateTransactionAsync(UpdateTransactionDTO updateTransactionDTO, CancellationToken ct)
        {
            var transaction = await _context.Transactions
                .Where(t => t.TransactionId == updateTransactionDTO.TransactionId && t.UserId == updateTransactionDTO.UserId)
                .FirstOrDefaultAsync(ct);

            if( transaction is null) 
                return null;

            transaction.TransactionName = updateTransactionDTO.TransactionName;
            transaction.Amount = updateTransactionDTO.Amount;
            transaction.TransactionDate = updateTransactionDTO.TransactionDate;
            transaction.Note = updateTransactionDTO.Note;   
            transaction.CategoryId = updateTransactionDTO.CategoryId;

            await _context.SaveChangesAsync(ct);

            return (new GetTransactionDTO
            {
                TransactionId = transaction.TransactionId,
                TransactionName = transaction.TransactionName,
                Amount = transaction.Amount,
                TransactionDate = transaction.TransactionDate,
                Note = transaction.Note,
                CategoryId = transaction.CategoryId,
                UserId = transaction.UserId,
            });
        }

        public async Task<PagedList<GetTransactionDTO>> GetTransactionsPagedAsync(TransactionParameters transactionParameters, CancellationToken ct)
        {
            var query = _context.Transactions
                .Where(t => t.UserId == transactionParameters.UserId)
                .AsQueryable();

            var count = 0;

            //SEARCH TERM
            if (!string.IsNullOrEmpty(transactionParameters.SearchTerm))
            {
                query = query.Where(q => q.TransactionName.Contains(transactionParameters.SearchTerm));
            }

            count = await query.CountAsync(ct);

            var result = await query
                .AsNoTracking()
                .OrderBy(q => q.TransactionDate)
                .Skip((transactionParameters.PageNumber - 1) * transactionParameters.PageSize)
                .Take(transactionParameters.PageSize)
                .Select(q => new GetTransactionDTO
                {
                    TransactionId = q.TransactionId,
                    TransactionName = q.TransactionName,
                    Amount = q.Amount,
                    TransactionDate = q.TransactionDate,
                    Note = q.Note,
                    CategoryId = q.CategoryId,
                    CategoryName = q.Category.CategoryName,
                    UserId = q.UserId,
                })
                .ToListAsync(ct);

            return PagedList<GetTransactionDTO>
                .ToPagedList(result, count, transactionParameters.PageNumber, transactionParameters.PageSize);
        }
    }
}
