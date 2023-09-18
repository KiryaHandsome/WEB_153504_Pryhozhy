using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Diagnostics;
using WEB_153504_Pryhozhy.IdentityServer.Models;

namespace WEB_153504_Pryhozhy.IdentityServer.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class AvatarController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;

        public AvatarController(IWebHostEnvironment environment, UserManager<ApplicationUser> userManager)
        {
            _environment = environment;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var imagesFolderPath = Path.Combine(_environment.ContentRootPath, "Images");
            var avatarPath = Path.Combine(imagesFolderPath, userId);
            avatarPath += ".jpg";

            if (System.IO.File.Exists(avatarPath))
            {
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(avatarPath, out var contentType))
                {
                    contentType = "application/octet-stream"; // MIME-тип по умолчанию
                }
                var stream = new FileStream(avatarPath, FileMode.Open, FileAccess.Read);
                return File(stream, contentType);
            }
            else
            {
                var placeholderPath = Path.Combine(imagesFolderPath, "default-profile-picture.png");

                if (System.IO.File.Exists(placeholderPath))
                {
                    var provider = new FileExtensionContentTypeProvider();
                    if (!provider.TryGetContentType(placeholderPath, out var contentType))
                    {
                        contentType = "application/octet-stream";
                    }

                    var stream = new FileStream(placeholderPath, FileMode.Open, FileAccess.Read);
                    return File(stream, contentType);
                }
                else
                {
                    return NotFound("Изображение не найдено.");
                }
            }
        }
    }
}
