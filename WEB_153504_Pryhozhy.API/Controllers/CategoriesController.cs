using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_153504_Pryhozhy.API.Services.CategoryService;
using WEB_153504_Pryhozhy.Domain.Entities;
using WEB_153504_Pryhozhy.Domain.Models;

namespace WEB_153504_Pryhozhy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseData<ListModel<Category>>>> GetCategories()
        {
            var response = await _categoryService.GetCategoryListAsync();

            return Ok(response);
        }
    }
}
