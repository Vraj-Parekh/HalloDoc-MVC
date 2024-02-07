using HalloDoc_Project.DataContext;
using HalloDoc_Project.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace HalloDoc_Project.Controllers
{
    public class PatientController : Controller
    {
        private readonly HalloDocDbContext _context;

        public PatientController(HalloDocDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PatientLogin() {
            return View();
        }

        [HttpPost]
        public IActionResult PatientLogin(LoginDTO data)
        {

            var user = _context.Aspnetusers.FirstOrDefault(u => u.Email == data.Email);
            //ModelState.AddModelError(key: "", "Not exist");
            //return View(data);

            if (user != null && user.Passwordhash == data.Password) // Check if user exists and password matches
            {
                // Redirect to the next page (replace "NextPageAction" with the appropriate action)
                return RedirectToAction("SubmitRequest", "Request");
            }
            else
            {
                // Redirect back to the login page
                return RedirectToAction("PatientLogin");
            }
           
        }

        public IActionResult ResetPwd()
        {
            return View();
        }
    }
}