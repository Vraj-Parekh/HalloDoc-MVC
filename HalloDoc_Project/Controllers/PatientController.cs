using Entities.DataContext;
using Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace HalloDoc_Project.Controllers
{
    public class PatientController : Controller
    {
        private readonly HalloDocDbContext _context;
        private readonly IWebHostEnvironment env;

        public PatientController(HalloDocDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
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
                HttpContext.Session.SetString("email", user.Email);
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
            string email = HttpContext.Session.GetString("email");
            List<PatientRequestList> data = new();
            var patientData = _context.Requests.Where(a => a.Email == email).Include(a => a.Requestwisefiles);

            foreach (var patientRequest in patientData)
            {
                PatientRequestList obj = new()
                {
                    CreatedDate = patientRequest.Createddate,
                    CurrentStatus = (RequestStatus)patientRequest.Status,
                    Document = patientRequest.Requestwisefiles.Count,
                    RequestId = patientRequest.Requestid
                };
                data.Add(obj);
            }
            return View(data);
        }

        public IActionResult ViewDocument(int requestId)
        {
            var file = _context.Requestwisefiles.Where(a => a.Requestid == requestId);
            var req = _context.Requests.Where(a => a.Requestid == requestId).FirstOrDefault();
            var name = _context.Requestclients.Where(a => a.Requestid == requestId).FirstOrDefault();
            List<FileData> data = new();

            foreach (var files in file)
            {
                FileData FileDataList = new()
                {
                    FileName = files.Filename,
                    CreatedBy = name.Firstname,
                    CreatedDate = files.Createddate,
                    DocumentId = files.Requestwisefileid
                };
                data.Add(FileDataList);
            }
            ViewDocumentList doc = new()
            {
                Name = name.Firstname,
                ConfirmationNumber = req.Confirmationnumber,
                Document = data
            };
            return View(doc);
        }

        public IActionResult Download(int download)
        {
            string filePath = getPath(download);

            return PhysicalFile(filePath, MimeTypes.GetMimeType(filePath), Path.GetFileName(filePath));


        }
        public string getPath(int download)
        {
            var file = _context.Requestwisefiles.Where(a => a.Requestwisefileid == download).FirstOrDefault();

            var uploads = Path.Combine(env.WebRootPath, "uploads");
            var filePath = Path.Combine(uploads, file.Filename);

            return filePath;
        }

        public IActionResult Profile()
        {
            var email = HttpContext.Session.GetString("email");
            var patientData = _context.Users.FirstOrDefault(a => a.Email == email);

            if (patientData == null)
                return NotFound();

            var patientProfile = new PatientProfileDTO()
            {
                FirstName = patientData.Firstname,
                LastName = patientData.Lastname,
                PhoneNumber = patientData.Mobile,
                Email = patientData.Email,
                Street = patientData.Street,
                City = patientData.City,
                State = patientData.State,
                ZipCode = patientData.Zipcode,
            };
            return View(patientProfile);
        }

        [HttpPost]
        public IActionResult Profile(PatientProfileDTO data)
        {
            if (ModelState.IsValid)
            {
                var email = HttpContext.Session.GetString("email");
                var patientData = _context.Users.FirstOrDefault(a => a.Email == email);

                patientData.Firstname = data.FirstName;
                patientData.Lastname = data.LastName;
                patientData.Mobile = data.PhoneNumber;
                patientData.Email = data.Email;
                patientData.Street = data.Street;
                patientData.City = data.City;
                patientData.State = data.State;
                patientData.Zipcode = data.ZipCode;
                patientData.Intdate = data.DateOfBirth.Day;
                patientData.Strmonth = data.DateOfBirth.ToString("MMM");
                patientData.Intyear = data.DateOfBirth.Year;

                _context.SaveChanges();

                return RedirectToAction("PatientDashboard");
            }
            return View(data);
        }
        public IActionResult ReviewAgreement()
        {
            return View();
        }

        public IActionResult CreateAccount()
        {
            return View();
        }

        public IActionResult RequestforMe()
        {
            return View();
        }

        public IActionResult RequestForSomeoneElse()
        {
            return View();
        }
    }
}