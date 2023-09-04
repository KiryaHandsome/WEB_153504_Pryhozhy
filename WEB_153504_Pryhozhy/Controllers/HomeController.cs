using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_153504_Pryhozhy.Models;

namespace WEB_153504_Pryhozhy.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            List<ListItemModel> demoList = new List<ListItemModel>
            {
                new ListItemModel { Id = 1, Name = "Item1" },
                new ListItemModel { Id = 2, Name = "Item2" },
                new ListItemModel { Id = 3, Name = "Item3" }
            };

            SelectList selectList = new SelectList(demoList, "Id", "Name");

            return View(selectList);
        }
    }
}
