using Microsoft.AspNetCore.Mvc;

namespace HalloDoc_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Back()
        {
            HttpRequest? path = HttpContext.Request;
            return Redirect(HttpContext.Request.Headers.Referer!);
        }
    }
}