using HalloDoc_Project.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult PatientLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PatientLogin(LoginDTO data)
        {

            var user = _context.Aspnetusers.FirstOrDefault(u => u.Email == data.Email);

            if (user != null && user.Passwordhash == data.Password)
            {
                return RedirectToAction("PatientDashboard", "Patient");
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

        public IActionResult PatientDashboard()
        {
            string email = "vraj@gmail.com";
            List<PatientRequestList> data = new List<PatientRequestList>();
            var patientData = _context.Requests.Where(a => a.Email == email).Include(a=>a.Requestwisefiles);

            foreach (var patientRequest in patientData)
            {
                PatientRequestList obj = new PatientRequestList()
                {
                    CreatedDate = patientRequest.Createddate,
                    CurrentStatus = (RequestStatus)patientRequest.Status,
                    Document = patientRequest.Requestwisefiles.Count
                };
                data.Add(obj);
            }

            return View(data);
        }
    }
}