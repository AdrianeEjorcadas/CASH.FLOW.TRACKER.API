namespace CASH.FLOW.TRACKER.API.Model.DTO.Categories
{
    public class UpdateCategoryDTO
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryType { get; set; }
        public Guid? UserId { get; set; }
    }
}
