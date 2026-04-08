using CASH.FLOW.TRACKER.API.Helpers.Pagination;

namespace CASH.FLOW.TRACKER.API.Model.DTO.Categories
{
    public class GetCategoriesPagedDTO
    {
        public  IEnumerable<GetCategoryDTO> Categories { get; set; }
        public Metadata Metadata { get; set; }
    }
}
