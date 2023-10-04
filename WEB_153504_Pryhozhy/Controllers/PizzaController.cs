using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_153504.Extensions;
using WEB_153504_Pryhozhy.Domain.Entities;
using WEB_153504_Pryhozhy.Services.CategoryService;
using WEB_153504_Pryhozhy.Services.PizzaService;


namespace WEB_153504_Pryhozhy.Controllers
{
    [Route("catalog")]
    public class PizzaController : Controller
    {
        private readonly IPizzaService _pizzaService;
        private readonly ICategoryService _categoryService;

        public PizzaController(ICategoryService categoryService, IPizzaService pizzaService)
        {
            _pizzaService = pizzaService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string? category, int pageNo = 1)
        {
            var productResponse = await _pizzaService.GetPizzaListAsync(category, pageNo);
            var categoryResponse = await _categoryService.GetCategoryListAsync();
            if (categoryResponse.Success)
            {
                ViewData["categories"] = categoryResponse.Data?.Items;
            }
            if (!productResponse.Success)
            {
                return NotFound(productResponse.ErrorMessage);
            }

            ViewData["currentCategory"] = GetCategoryNameOrDefault(category, categoryResponse.Data.Items);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_CatalogPartial", productResponse.Data);
            }

            return View(productResponse.Data);
        }

        private string GetCategoryNameOrDefault(string? categoryNormalizedName, List<Category> categories)
        {
            if (categoryNormalizedName == null)
            {
                return "Все";
            }
            else
            {
                return categories.Find(c => c.NormalizedName == categoryNormalizedName).Name;
            }
        }
    }
}
