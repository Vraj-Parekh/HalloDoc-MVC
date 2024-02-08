using HalloDoc_Project.DTO;
using Microsoft.AspNetCore.Mvc;

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

            if (user != null && user.Passwordhash == data.Password)
            {
                return RedirectToAction("SubmitRequest", "Request");
            }
            else
            {
                return RedirectToAction("PatientLogin");
            }
           
        }

        public IActionResult ResetPwd()
        {
            return View();
        }
    }
}