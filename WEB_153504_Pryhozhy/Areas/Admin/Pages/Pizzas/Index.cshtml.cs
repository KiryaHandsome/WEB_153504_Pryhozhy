using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_153504_Pryhozhy.Domain.Entities;
using WEB_153504_Pryhozhy.Domain.Models;
using WEB_153504_Pryhozhy.Services.PizzaService;

namespace WEB_153504_Pryhozhy.Areas.Admin.Pages.Pizzas
{
    public class IndexModel : PageModel
    {
        private readonly IPizzaService _pizzaService;

        public IndexModel(IPizzaService pizzaService)
        {
            _pizzaService = pizzaService;
        }

        public ListModel<Pizza> Pizza { get; set; } = new ListModel<Pizza>();

        public async Task OnGetAsync(int pageNo = 1)
        {
            var response = await _pizzaService.GetPizzaListAsync(null, pageNo);
            if (response.Success)
            {
                Pizza = response.Data;
            }
        }
    }
}
