using Microsoft.AspNetCore.Mvc;

namespace HalloDoc_Project.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AdminDashboard()
        {
            return View();  
        }
    }
}
