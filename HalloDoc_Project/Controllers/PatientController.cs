using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using HalloDoc.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using NuGet.Protocol.Plugins;
using Repositories.Repository.Implementation;
using Repositories.Repository.Interface;
using System.Globalization;
using System.Threading.Tasks.Dataflow;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HalloDoc_Project.Controllers
{
    public class PatientController : Controller
    {
        private readonly HalloDocDbContext _context;
        private readonly IWebHostEnvironment env;
        private readonly IEmailSender emailSender;
        private readonly IRequestServices requestServices;

        public PatientController(HalloDocDbContext context, IWebHostEnvironment env, IEmailSender emailSender, IRequestServices requestServices)
        {
            _context = context;
            this.env = env;
            this.emailSender = emailSender;
            this.requestServices = requestServices;
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
                ModelState.AddModelError(nameof(data.Email), "An account with this email does not exists.");
                return View(data);
            }

        }

        public IActionResult ResetPwd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPwd(LoginDTO data)
        {
            if (_context.Aspnetusers.Any(u => u.Email == data.Email))
            {
                emailSender.SendEmailAsync(data.Email, "Reset Password", $"Tap the link to reset the password: <a href=\"https://localhost:44396/patient/changepassword/{data.Email}\">Reset now</a>");
                return RedirectToAction("PatientLogin", "Patient");
            }
            else
            {
                throw new Exception("User Not Found");
            }
        }

        [HttpGet("[controller]/[action]/{email}")]
        public IActionResult ChangePassword(string email)
        {
            TempData["email"] = email;
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(LoginDTO data)
        {
            string email = TempData["email"] as string;

            var aspNetUserData = _context.Aspnetusers.FirstOrDefault(a => a.Email == email);

            aspNetUserData.Passwordhash = data.ConfirmPassword;

            _context.Aspnetusers.Update(aspNetUserData);
            _context.SaveChanges();

            return RedirectToAction("PatientLogin");
        }
        public IActionResult PatientDashboard()
        {
            string email = HttpContext.Session.GetString("email");
            List<PatientRequestList> data = new();

            var patientData = _context.Requests.Where(a => a.Email == email && a.Requesttypeid == 2).Include(a => a.Requestwisefiles);

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
            TempData["requestId"] = requestId;
            var file = _context.Requestwisefiles.Where(a => a.Requestid == requestId);
            var req = _context.Requests.Where(a => a.Requestid == requestId).FirstOrDefault();
            var name = _context.Requestclients.Where(a => a.Requestid == requestId).FirstOrDefault();
            List<FileData> data = new();

            if (file is not null)
            {
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
        private DateTime GenerateDateOfBirth(int? year, string? month, int? date)
        {

            DateTime finalDate = new DateTime(year ?? 1900, DateTime.ParseExact(month ?? "January", "MMMM", CultureInfo.CurrentCulture).Month, date ?? 01);
            return finalDate;
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
                DateOfBirth = GenerateDateOfBirth(patientData.Intyear,patientData.Strmonth,patientData.Intdate)
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
                patientData.Strmonth = data.DateOfBirth.ToString("MMMM");
                patientData.Intyear = data.DateOfBirth.Year;

                _context.Users.Update(patientData);
                _context.SaveChanges();

                return RedirectToAction("Profile");
            }
            return View(data);
        }


        [HttpGet("[controller]/[action]/{requestId:int}")]
        public IActionResult ReviewAgreement(int requestId)
        {
            string? email = HttpContext.Session.GetString("Email");
            bool requestPending = requestServices.IsRequestPending(requestId, email);
            if (requestPending)
            {
                ViewBag.RequestId = requestId;
                return View();
            }
            return RedirectToAction("PatientDashboard");
        }

        public async Task<IActionResult> AgreeAgreement(int requestId)
        {
            bool result = await requestServices.AgreeWithAgreementAsync(requestId);
            if (result)
            {
                return RedirectToAction("PatientDashboard");
            }
            return RedirectToAction("ReviewAgreement", new { requestId = requestId });
        }
         
        [HttpPost]
        public async Task<IActionResult> RejectAgreement(AgreementDTO model)
        {
            bool result = await requestServices.RejectAgreementAsync(model.RequestId, model.Notes);
            if (result)
            {
                return RedirectToAction("PatientDashboard");
            }
            return RedirectToAction("ReviewAgreement", new { requestId = model.RequestId });
        }

        [HttpGet("[controller]/[action]/{requestId:int}")]
        public IActionResult CreateAccount(int requestId)
        {
            TempData["requestId"] = requestId;
            return View();
        }

        [HttpPost]
        public IActionResult CreateAccount(LoginDTO data)
        {
            if (ModelState.IsValid)
            {
                int requestId = (int)TempData["requestId"];

                var requestClient = _context.Requestclients.FirstOrDefault(u => u.Requestid == requestId);

                if (requestClient is null)
                {
                    ModelState.AddModelError("", "No request associated with this email address");
                    return View(data);
                }

                int requestClientId = requestClient.Requestclientid;

                if (requestClient.Email != data.Email)
                {
                    ModelState.AddModelError(string.Empty, "Wrong email");
                    return View(data);
                }

                var aspNetUser = new Aspnetuser
                {
                    Username = data.Email,
                    Passwordhash = data.Password,
                    Email = data.Email,
                    Aspnetuserid = Guid.NewGuid().ToString(),
                    Createddate = DateTime.Now,
                };

                var user = new User
                {
                    Firstname = aspNetUser.Username,
                    Email = aspNetUser.Email,
                    Createdby = "patient",
                    Createddate = DateTime.Now,
                    Aspnetuser = aspNetUser,
                };

                _context.Aspnetusers.Add(aspNetUser);
                _context.Users.Add(user);
                _context.SaveChanges();

                var requestData = _context.Requests.FirstOrDefault(a => a.Requestid == requestId);

                requestData.User = user;
                requestData.Createduserid = user.Userid;
                requestData.Patientaccountid = aspNetUser.Aspnetuserid;

                _context.Requests.Update(requestData);
                try
                {
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Error creating account. Please try again later.");
                    return View(data);
                }
            }
            else
            {
                return View(data);
            }
        }


        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 6)
                      + Path.GetExtension(fileName);
        }
        private string GetConfirmationNumber(string city, string lastname, string firstname, string count)
        {
            string regionAbr = city.Substring(0, 2).ToUpper();
            string date = DateTime.Now.ToString("dd");
            string month = DateTime.Now.ToString("MM");
            string last = lastname.Substring(0, 2).ToUpper();
            string first = firstname.Substring(0, 2).ToUpper();
            string requestCount = count;

            return regionAbr + date + month + last + first + requestCount;
        }

        public IActionResult RequestforMe()
        {
            var email = HttpContext.Session.GetString("email");
            var patientData = _context.Users.FirstOrDefault(a => a.Email == email);

            if (patientData == null)
                return NotFound();

            var patientProfile = new PatientRequestDTO()
            {
                FirstName = patientData.Firstname,
                LastName = patientData.Lastname,
                PhoneNumber = patientData.Mobile,
                Email = patientData.Email,
                DateOfBirth = GenerateDateOfBirth(patientData.Intyear, patientData.Strmonth, patientData.Intdate)
            };
            return View(patientProfile);
        }

        [HttpPost]
        public IActionResult RequestforMe(PatientRequestDTO data)
        {
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            if (ModelState.IsValid)
            {
                string count = _context.Requests.Where(a => a.Createddate.Date == DateTime.Now.Date).Count().ToString("0000");
                var request = new Request
                {
                    Requesttypeid = 2,
                    Firstname = data.FirstName,
                    Lastname = data.LastName,
                    Phonenumber = data.PhoneNumber,
                    Email = data.Email,
                    Status = (int)RequestStatus.Unassigned,
                    Createddate = DateTime.Now,
                    Isurgentemailsent = false,
                    Confirmationnumber = GetConfirmationNumber(data.City, data.LastName, data.FirstName, count),
                };

                var requestClient = new Requestclient
                {
                    Firstname = data.FirstName,
                    Lastname = data.LastName,
                    Intdate = data.DateOfBirth.Day,
                    Intyear = data.DateOfBirth.Year,
                    Strmonth = data.DateOfBirth.ToString("MMM"),
                    Email = data.Email,
                    Street = data.Street,
                    City = data.City,
                    State = data.State,
                    Zipcode = data.ZipCode,
                    Request = request
                };

                //to get uploaded files in the 'uploads' folder
                if (data.File is not null)
                {

                    foreach (var item in data?.File)
                    {
                        var file = item;
                        var uniqueFileName = GetUniqueFileName(file.FileName);
                        var uploads = Path.Combine(env.WebRootPath, "uploads");
                        var filePath = Path.Combine(uploads, uniqueFileName);
                        file.CopyTo(new FileStream(filePath, FileMode.Create));

                        var requestWiseFile = new Requestwisefile
                        {
                            Createddate = DateTime.Now,
                            Filename = uniqueFileName,
                            Request = request,
                        };
                        _context.Requestwisefiles.Add(requestWiseFile);
                    }
                }


                try
                {
                    _context.Requests.Add(request);
                    _context.Requestclients.Add(requestClient);
                    _context.SaveChanges();

                    return RedirectToAction("PatientDashboard", "Patient");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while processing your request.");
                    return RedirectToAction("FamilyInfo", "Request");
                }
            }
            return View(data);

        }

        public IActionResult RequestForSomeoneElse()
        {
            var email = HttpContext.Session.GetString("email");
            var user = _context.Users.FirstOrDefault(a => a.Email == email);

            FamilyRequestDTO model = new()
            {
                FirstName = user.Firstname,
                LastName = user.Lastname,
                Email = user.Email,
                Phone = user.Mobile,
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult RequestForSomeoneElse(FamilyRequestDTO data)
        {
            if (ModelState.IsValid)
            {
                string count = _context.Requests.Where(a => a.Createddate.Date == DateTime.Now.Date).Count().ToString("0000");
                var request = new Request
                {
                    Requesttypeid = 2,
                    Firstname = data.FirstName,
                    Lastname = data.LastName,
                    Phonenumber = data.PatientPhone,
                    Email = data.Email,
                    Status = (int)RequestStatus.Unassigned,
                    Createddate = DateTime.Now,
                    Isurgentemailsent = false,
                    Confirmationnumber = GetConfirmationNumber(data.City, data.LastName, data.FirstName, count),
                    Relationname = data.RelationWithPatient,
                };

                var requestClient = new Requestclient
                {
                    Firstname = data.FirstName,
                    Lastname = data.LastName,
                    Intdate = data.DateOfBirth.Day,
                    Intyear = data.DateOfBirth.Year,
                    Strmonth = data.DateOfBirth.ToString("MMM"),
                    Email = data.Email,
                    Street = data.Street,
                    City = data.City,
                    State = data.State,
                    Zipcode = data.ZipCode,
                    Request = request
                };

                //to get uploaded files in the 'uploads' folder
                if (data.File is not null)
                {

                    foreach (var item in data.File)
                    {
                        var file = item;
                        var uniqueFileName = GetUniqueFileName(file.FileName);
                        var uploads = Path.Combine(env.WebRootPath, "uploads");
                        var filePath = Path.Combine(uploads, uniqueFileName);
                        file.CopyTo(new FileStream(filePath, FileMode.Create));

                        var requestWiseFile = new Requestwisefile
                        {
                            Createddate = DateTime.Now,
                            Filename = uniqueFileName,
                            Request = request,
                        };
                        _context.Requestwisefiles.Add(requestWiseFile);
                    }

                }
                try
                {
                    _context.Requests.Add(request);
                    _context.Requestclients.Add(requestClient);
                    _context.SaveChanges();

                    return RedirectToAction("PatientDashboard", "Patient");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while processing your request.");
                    return RedirectToAction("FamilyInfo", "Request");
                }
            }
            return View(data);
        }
    }
}