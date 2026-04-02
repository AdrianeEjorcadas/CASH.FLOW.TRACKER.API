using CASH.FLOW.TRACKER.API.Model.DTO.Categories;
using CASH.FLOW.TRACKER.API.Model.Response;
using CASH.FLOW.TRACKER.API.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CASH.FLOW.TRACKER.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("category")]
        public async Task<ActionResult<ReturnResponse<object>>> AddCategoryAsync([FromBody] AddCategoryDTO addCategoryDT, CancellationToken ct = default)
        {
            await _categoryService.AddCategoryAsync(addCategoryDT, ct);

            return Ok(new ReturnResponse<object>
            {
                StatusCode = 200,
                Message = "Added succesfully",
                Data = null
            });
        }


        [HttpGet("category-by-id")]
        public async Task<ActionResult<ReturnResponse<GetCategoryDTO>>> GetCategoryByIdAsync([FromQuery] int categoryId, CancellationToken ct = default)
        {
            var payload = await _categoryService.GetCategoryByIdAsync(categoryId, ct);

            return Ok(new ReturnResponse<GetCategoryDTO>
            {
                StatusCode = 200,
                Message = "Retrieve successfully",
                Data = payload
            });
        }

        [HttpGet("categories")]
        public async Task<ActionResult<ReturnResponse<IEnumerable<GetCategoryDTO>>>> GetCategoriesAsync(CancellationToken ct = default)
        {
            var payload = await _categoryService.GetCategories(ct);

            return Ok(new ReturnResponse<IEnumerable<GetCategoryDTO>>
            {
                StatusCode = 200,
                Message = "Retrieve successfully",
                Data = payload
            });
        }

        [HttpDelete("category")]
        public async Task<ActionResult<ReturnResponse<object>>> DeleteCategoryAsync([FromQuery] int categoryId, CancellationToken ct = default)
        {
            await _categoryService.DeleteCategoryAsync(categoryId, ct);

            return Ok(new ReturnResponse<object>
            {
                StatusCode = 200,
                Message = "Successfully deleted",
                Data = null
            });
        }

        [HttpPatch("category")]
        public async Task<ActionResult<ReturnResponse<object>>> UpdateCategoryAsync([FromBody] UpdateCategoryDTO updateCategoryDTO, CancellationToken ct = default)
        {
            await _categoryService.UpdateCategoryAsync(updateCategoryDTO, ct);

            return Ok(new ReturnResponse<object>
            {
                StatusCode = 200,
                Message = "Updated successfully",
                Data = null
            });
        }

    }
}
