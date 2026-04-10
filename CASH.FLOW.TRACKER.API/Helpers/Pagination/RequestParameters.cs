namespace CASH.FLOW.TRACKER.API.Helpers.Pagination
{
    public abstract class RequestParameters
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        //Filters
        public string? SearchTerm { get; set; }
        public bool? IsArchived { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
