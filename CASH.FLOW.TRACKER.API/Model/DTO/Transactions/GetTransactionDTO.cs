namespace CASH.FLOW.TRACKER.API.Model.DTO.Transactions
{
    public class GetTransactionDTO
    {
        public Guid TransactionId { get; set; }
        public Guid UserId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string TransactionName { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
        public string? Note { get; set; }
        
    }
}
