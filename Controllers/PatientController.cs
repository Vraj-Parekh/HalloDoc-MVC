using Microsoft.AspNetCore.Mvc;

namespace HalloDoc_Project.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PatientLogin() {
            return View();
        }
        public IActionResult ResetPwd()
        {
            return View();
        }
        public IActionResult SubmitRequest()
        {
            return View();
        }
        public IActionResult PatientInfo()
        {
            return View();
        } public IActionResult FamilyInfo()
        {
            return View();
        } public IActionResult ConciergeInfo()
        {
            return View();
        } public IActionResult BusinessInfo()
        {
            return View();
        }
        
    }
}
