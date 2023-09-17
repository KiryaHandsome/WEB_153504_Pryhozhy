using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_153504_Pryhozhy.Domain.Entities;
using WEB_153504_Pryhozhy.Services.PizzaService;

namespace WEB_153504_Pryhozhy.Areas.Admin.Pages.Pizzas
{
    public class DetailsModel : PageModel
    {
        private readonly IPizzaService _pizzaService;

        public DetailsModel(IPizzaService pizzaService)
        {
            _pizzaService = pizzaService;
        }

        public Pizza Pizza { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var response = await _pizzaService.GetByIdAsync((int)id);
            if (response.Success) 
            {
                Pizza = response.Data;
            }
            else
            {
                return NotFound();
            }

            return Page();
        }
    }
}
