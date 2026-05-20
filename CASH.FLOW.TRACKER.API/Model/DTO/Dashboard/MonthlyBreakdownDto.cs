namespace CASH.FLOW.TRACKER.API.Model.DTO.Dashboard
{
    public class MonthlyBreakdownDto
    {
        public DateTimeOffset Month { get; set; }
        public IEnumerable<CategoryBreakdownDto> Categories { get; set; }
    }
}
