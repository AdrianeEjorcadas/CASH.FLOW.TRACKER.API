using CASH.FLOW.TRACKER.API.Helpers.Pagination;

namespace CASH.FLOW.TRACKER.API.Model.DTO.Transactions
{
    public class GetTransactionPagedDTO
    {
        public IEnumerable<GetTransactionDTO> Transactions { get; set; }
        public Metadata Metadata { get; set; }
    }
}
