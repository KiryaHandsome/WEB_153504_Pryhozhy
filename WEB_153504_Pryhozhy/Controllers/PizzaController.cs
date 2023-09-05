using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using WEB_153504_Pryhozhy.Domain.Entities;
using WEB_153504_Pryhozhy.Domain.Models;
using WEB_153504_Pryhozhy.Services.CategoryService;
using WEB_153504_Pryhozhy.Services.PizzaService;

namespace WEB_153504_Pryhozhy.Controllers
{
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
                
            return View(productResponse.Data);
        }
    }
}
