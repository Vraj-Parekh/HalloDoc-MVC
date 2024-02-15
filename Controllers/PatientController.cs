using HalloDoc_Project.DTO;
using HalloDoc_Project.Models;
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
            List<PatientRequestList> data = new();
            var patientData = _context.Requests.Where(a => a.Email == email).Include(a=>a.Requestwisefiles);

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
            var req = _context.Requests.Where(a=>a.Requestid == requestId).FirstOrDefault();
            var name = _context.Requestclients.Where(a => a.Requestid == requestId).FirstOrDefault();
            List<FileData> data = new();

            foreach(var files in file)
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
            var file = _context.Requestwisefiles.Where(a=>a.Requestwisefileid == download).FirstOrDefault();

            var uploads = Path.Combine(env.WebRootPath, "uploads");
            var filePath = Path.Combine(uploads, file.Filename);

            return filePath;
        }

        public IActionResult Profile(int requestId)
        {
            var firstName = _context.Requestclients
            return View();
        }
    }
}