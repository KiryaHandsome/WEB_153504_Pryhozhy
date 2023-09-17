using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_153504_Pryhozhy.Domain.Entities;
using WEB_153504_Pryhozhy.Services.PizzaService;

namespace WEB_153504_Pryhozhy.Areas.Admin.Pages.Pizzas
{
    public class DeleteModel : PageModel
    {
        private readonly IPizzaService _pizzaService;

        public DeleteModel(IPizzaService pizzaService)
        {
            _pizzaService = pizzaService;
        }

        [BindProperty]
        public Pizza Pizza { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _pizzaService.GetByIdAsync((int)id);

            if (!response.Success)
            {
                return NotFound();
            }

            Pizza = response.Data;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _pizzaService.DeleteAsync((int)id);

            return RedirectToPage("./Index");
        }
    }
}
