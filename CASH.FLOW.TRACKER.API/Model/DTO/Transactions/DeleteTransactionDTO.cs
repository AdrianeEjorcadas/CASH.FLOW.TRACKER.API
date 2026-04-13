namespace CASH.FLOW.TRACKER.API.Model.DTO.Transactions
{
    public class DeleteTransactionDTO
    {
        public Guid TransactionId { get; set; }
        public Guid UserId { get; set; }
    }
}
