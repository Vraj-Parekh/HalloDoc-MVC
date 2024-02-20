using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HalloDoc_Project.Controllers
{
    public class RequestController : Controller
    {
        private readonly HalloDocDbContext _context;
        private readonly IWebHostEnvironment env;

        public RequestController(HalloDocDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        public IActionResult Index()
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
        }

        private string GetConfirmationNumber(string city, string lastname, string firstname, string count)
        {
            string regionAbr = city.Substring(0, 2);
            string date = DateTime.Now.ToString("dd");
            string month = DateTime.Now.ToString("MM");
            string last = lastname.Substring(0, 2);
            string first = firstname.Substring(0, 2);
            string requestCount = count;

            return regionAbr + date + month + last + first + requestCount;
        }

        [HttpPost]
        public IActionResult PatientInfo(PatientRequestDTO data)
        {
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            if (ModelState.IsValid)
            {
                var isUserExist = _context.Aspnetusers.FirstOrDefault(u => u.Email == data.Email);
                if (isUserExist != null)
                {
                    //ModelState.AddModelError("", "An account with this email already exists.");
                    //return RedirectToAction("FamilyInfo", "Request");
                    return View(data);
                }

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
                var file = data.File;
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

                var aspNetUser = new Aspnetuser
                {
                    Aspnetuserid = Guid.NewGuid().ToString(),
                    Username = data.Email,
                    Passwordhash = data.ConfirmPassword,
                    Phonenumber = data.PhoneNumber,
                    Email = data.Email,
                    Createddate = DateTime.Now,
                };

                var user = new User
                {
                    Firstname = data.FirstName,
                    Lastname = data.LastName,
                    Mobile = data.PhoneNumber,
                    Email = data.Email,
                    Intdate = data.DateOfBirth.Day,
                    Intyear = data.DateOfBirth.Year,
                    Strmonth = data.DateOfBirth.ToString("MMM"),
                    Street = data.Street,
                    City = data.City,
                    State = data.State,
                    Zipcode = data.ZipCode,
                    Createddate = DateTime.Now,
                    Createdby = "patient",
                    Aspnetuser = aspNetUser
                };

                try
                {
                    _context.Requests.Add(request);
                    _context.Requestclients.Add(requestClient);
                    _context.Users.Add(user);
                    _context.Requestwisefiles.Add(requestWiseFile);
                    _context.Aspnetusers.Add(aspNetUser);
                    _context.SaveChanges();

                    return RedirectToAction("SubmitRequest", "Request");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while processing your request.");
                    return RedirectToAction("FamilyInfo", "Request");
                }
            }
            return View(data);

        }
        public IActionResult FamilyInfo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FamilyInfo(FamilyRequestDTO data)
        {
            if (ModelState.IsValid)
            {
                var isUserExist = _context.Aspnetusers.FirstOrDefault(u => u.Email == data.PatientEmail);
                if (isUserExist != null)
                {
                    //ModelState.AddModelError("", "An account with this email already exists.");
                    //return RedirectToAction("FamilyInfo", "Request");
                    return View(data);
                }

                string count = _context.Requests.Where(a => a.Createddate.Date == DateTime.Now.Date).Count().ToString("0000");
                var request = new Request
                {
                    Requesttypeid = 3,
                    Firstname = data.FirstName,
                    Lastname = data.LastName,
                    Phonenumber = data.Phone,
                    Email = data.Email,
                    Status = (int)RequestStatus.Unassigned,
                    Createddate = DateTime.Now,
                    Isurgentemailsent = false,
                    Confirmationnumber = GetConfirmationNumber(data.City, data.LastName, data.FirstName, count),
                };

                var requestClient = new Requestclient
                {
                    Firstname = data.PatientFirstName,
                    Lastname = data.PatientLastName,
                    Email = data.PatientEmail,
                    Intdate = data.DateOfBirth.Day,
                    Intyear = data.DateOfBirth.Year,
                    Strmonth = data.DateOfBirth.ToString("MMM"),
                    Street = data.Street,
                    City = data.City,
                    State = data.State,
                    Zipcode = data.ZipCode,
                    Request = request
                };

                //to get uploaded files in the 'uploads' folder
                var file = data.File;
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

                try
                {
                    _context.Requests.Add(request);
                    _context.Requestclients.Add(requestClient);
                    _context.Requestwisefiles.Add(requestWiseFile);
                    _context.SaveChanges();

                    return RedirectToAction("SubmitRequest", "Request");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while processing your request.");
                    return RedirectToAction("FamilyInfo", "Request");
                }
            }
            return View(data);
        }

        public IActionResult ConciergeInfo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ConciergeInfo(ConciergeRequestDTO data)
        {
            if (ModelState.IsValid)
            {
                var isUserExist = _context.Aspnetusers.FirstOrDefault(u => u.Email == data.PatientEmail);
                if (isUserExist != null)
                {
                    //ModelState.AddModelError("", "An account with this email already exists.");
                    //return RedirectToAction("FamilyInfo", "Request");
                    return View(data);
                }

                string count = _context.Requests.Where(a => a.Createddate.Date == DateTime.Now.Date).Count().ToString("0000");
                var request = new Request
                {
                    Requesttypeid = 4,
                    Firstname = data.FirstName,
                    Lastname = data.LastName,
                    Phonenumber = data.Phone,
                    Email = data.Email,
                    Status = (int)RequestStatus.Unassigned,
                    Createddate = DateTime.Now,
                    Isurgentemailsent = false,
                    Confirmationnumber = GetConfirmationNumber(data.City, data.LastName, data.FirstName, count)
                };

                var requestClient = new Requestclient
                {
                    Firstname = data.PatientFirstName,
                    Lastname = data.PatientLastName,
                    Email = data.PatientEmail,
                    Intdate = data.DateOfBirth.Day,
                    Intyear = data.DateOfBirth.Year,
                    Strmonth = data.DateOfBirth.ToString("MMM"),
                    Street = data.Street,
                    City = data.City,
                    State = data.State,
                    Zipcode = data.ZipCode,
                    Request = request
                };

                var concierge = new Concierge
                {
                    Conciergename = data.FirstName,
                    Street = data.Street,
                    City = data.City,
                    State = data.State,
                    Zipcode = data.ZipCode,
                    Createddate = DateTime.Now,
                    Regionid = 1,
                };

                var requestConcierge = new Requestconcierge
                {
                    Request = request,
                    Concierge = concierge
                };

                try
                {
                    _context.Requests.Add(request);
                    _context.Requestclients.Add(requestClient);
                    _context.SaveChanges();

                    return RedirectToAction("SubmitRequest", "Request");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while processing your request.");
                    return RedirectToAction("FamilyInfo", "Request");
                }
            }
            return View(data);

        }
        public IActionResult BusinessInfo()
        {
            return View();
        }

        [Route("[controller]/[action]/{email}")]
        public bool IsPatientPresent(string email)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == email);

            if (user is null)
            {
                return false;
            }
            return true;
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 6)
                      + Path.GetExtension(fileName);
        }

       

    }
}
