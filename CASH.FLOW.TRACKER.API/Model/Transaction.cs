using CASH.FLOW.TRACKER.API.Model;

namespace BUGET.TRACKER.API.Model
{
    public class Transaction
    {
        public Guid TransactionId { get; set; }
        public Guid UserId { get; set; }
        public int CategoryId { get; set; }
        public string TransactionName { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
        public string? Note { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        // Navigation property: one transaction belongs to one category
        public Category Category { get; set; }
        public ApplicationUser User { get; set; }
    }
}
