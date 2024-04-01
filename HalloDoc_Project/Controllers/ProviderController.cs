using HalloDoc_Project.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc_Project.Controllers
{
    [Route("[controller]/[action]")]
    [CustomAuthorization("Admin, Provider")]
    public class ProviderController : Controller
    {
        public IActionResult CreateProviderAccount()
        {
            return View();
        }
    }
}
