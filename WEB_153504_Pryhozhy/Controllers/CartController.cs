using Microsoft.AspNetCore.Mvc;
using WEB_153504_Pryhozhy.Domain.Entities;
using WEB_153504_Pryhozhy.Services.PizzaService;

namespace WEB_153504_Pryhozhy.Controllers
{
    public class CartController : Controller
    {
        private readonly IPizzaService _pizzaService;
        private readonly Cart _cart;

        public CartController(IPizzaService pizzaService, Cart cart)
        {
            _pizzaService = pizzaService;
            _cart = cart;
        }

        public IActionResult Index()
        {
            return View(_cart);
        }

        [Route("[controller]/add/{id:int}")]
        public async Task<ActionResult> Add(int id, string returnUrl)
        {
            var data = await _pizzaService.GetByIdAsync(id);
            if (data.Success)
            {
                _cart.AddToCart(data.Data!);
            }
            return Redirect(returnUrl);
        }

        public IActionResult RemoveItem(int id, string redirectUrl)
        {
            _cart.RemoveItems(id);
            return Redirect(redirectUrl);
        }
    }
}
