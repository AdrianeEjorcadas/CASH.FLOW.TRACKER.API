namespace CASH.FLOW.TRACKER.API.Model.DTO.Transactions
{
    public class GetTransactionByIdDTO
    {
        public Guid TransactionID { get; set; }
        public Guid UserId { get; set; }
    }
}
