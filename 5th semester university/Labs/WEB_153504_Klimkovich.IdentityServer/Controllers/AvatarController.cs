using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using WEB_153504_Klimkovich.IdentityServer.Models;

namespace WEB_153504_Klimkovich.IdentityServer.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class AvatarController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;
        public AvatarController(IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _environment = webHostEnvironment;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var imagesFolderPath = Path.Combine(_environment.ContentRootPath, "Images");
            var avatarPath = Path.Combine(imagesFolderPath, userId + ".jpg");

            if (!System.IO.File.Exists(avatarPath))
            {
                avatarPath = Path.Combine(imagesFolderPath, "defaultIcon.png");

                if (!System.IO.File.Exists(avatarPath))
                {
                    return NotFound("Image not found");
                }
            }

            if (!new FileExtensionContentTypeProvider().TryGetContentType(avatarPath, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            var stream = new FileStream(avatarPath, FileMode.Open, FileAccess.Read);
            return File(stream, contentType);
        }
    }
}
