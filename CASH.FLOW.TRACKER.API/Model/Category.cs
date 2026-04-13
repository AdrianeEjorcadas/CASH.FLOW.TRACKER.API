namespace BUGET.TRACKER.API.Model
{
    public class Category
    {
        public int CategoryId { get; set; }
        public Guid UserId { get; set; }
        public string CategoryName { get; set; } //Food, Rent, Salary
        public string CategoryType { get; set; } //Income or Expense
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        // Navigation property: one category has many transactions
        public ICollection<Transaction> Transactions { get; set; }
    }
}
