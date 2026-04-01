using CASH.FLOW.TRACKER.API.Model.DTO;
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
        public async Task<ActionResult<ReturnResponse<object>>> AddCategoryAsync(AddCategoryDTO addCategoryDT, CancellationToken ct = default)
        {
            await _categoryService.AddCategoryAsync(addCategoryDT, ct);

            return Ok(new ReturnResponse<object>
            {
                StatusCode = 200,
                Message = "Added succesfully",
                Data = null
            });
        }
    }
}
