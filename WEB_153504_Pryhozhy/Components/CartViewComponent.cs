using Microsoft.AspNetCore.Mvc;
using WEB_153504_Pryhozhy.Domain.Entities;

namespace WEB_153504_Pryhozhy.Components
{
    public class CartViewComponent : ViewComponent
    {
        public Cart Cart { get; set; }

        public CartViewComponent(Cart cart)
        {
            Cart = cart;
        }

        public IViewComponentResult Invoke()
        {
            return View(Cart);
        }
    }
}
