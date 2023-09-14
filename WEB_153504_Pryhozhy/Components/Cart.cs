using Microsoft.AspNetCore.Mvc;

namespace WEB_153504_Pryhozhy.Components
{
    [ViewComponent]
    public class Cart : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
