namespace CASH.FLOW.TRACKER.API.Model.DTO.Categories
{
    public class GetCategoryDTO
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } //Food, Rent, Salary
        public string CategoryType { get; set; } //Income or Expense
        public Guid UserId { get; set; }
    }
}
