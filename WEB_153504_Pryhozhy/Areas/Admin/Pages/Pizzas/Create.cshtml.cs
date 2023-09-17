using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using WEB_153504_Pryhozhy.Domain.Entities;
using WEB_153504_Pryhozhy.Services.CategoryService;
using WEB_153504_Pryhozhy.Services.PizzaService;

namespace WEB_153504_Pryhozhy.Areas.Admin.Pages.Pizzas
{
    public class CreateModel : PageModel
    {
        private readonly IPizzaService _pizzaService;
        private readonly ICategoryService _categoryService;

        public CreateModel(IPizzaService prodctService, ICategoryService categoryService)
        {
            _pizzaService = prodctService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var categories = await _categoryService.GetCategoryListAsync();
            ViewData["categories"] = categories;
            return Page();
        }

        [BindProperty]
        public Pizza Pizza { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid || Pizza == null)
            {
                return Page();
            }

            await _pizzaService.CreateAsync(Pizza, Image);

            return RedirectToPage("./Index");
        }
    }
}
