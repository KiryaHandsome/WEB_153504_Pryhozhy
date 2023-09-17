using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using WEB_153504_Pryhozhy.Domain.Entities;
using WEB_153504_Pryhozhy.Services.CategoryService;
using WEB_153504_Pryhozhy.Services.PizzaService;

namespace WEB_153504_Pryhozhy.Areas.Admin.Pages.Pizzas
{
    public class EditModel : PageModel
    {
        private readonly IPizzaService _pizzaService;
        private readonly ICategoryService _categoryService;

        public EditModel(IPizzaService pizzaService, ICategoryService categoryService)
        {
            _pizzaService = pizzaService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public Pizza Pizza { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; } = null;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizza = await _pizzaService.GetByIdAsync((int)id);
            ViewData["categories"] = await _categoryService.GetCategoryListAsync();
            if (pizza == null)
            {
                return NotFound();
            }
            Pizza = pizza.Data;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _pizzaService.UpdateAsync(Pizza.Id, Pizza, Image);

            return RedirectToPage("./Index");
        }
    }
}
