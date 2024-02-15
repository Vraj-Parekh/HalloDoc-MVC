using HalloDoc_Project.DTO;
using HalloDoc_Project.Models;
using Microsoft.AspNetCore.Mvc;


namespace HalloDoc_Project.Controllers
{
    public class RequestController : Controller
    {
        private readonly HalloDocDbContext _context;
        private readonly IWebHostEnvironment env;

        public RequestController(HalloDocDbContext context,IWebHostEnvironment env)
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

        [HttpPost]
        public IActionResult PatientInfo(PatientRequestDTO data)
        {
            if (ModelState.IsValid)
            {
                var request = new Request
                {
                    Requesttypeid = 2,
                    Firstname = data.FirstName,
                    Lastname = data.LastName,
                    Phonenumber = data.PhoneNumber,
                    Email = data.Email,
                    Status = (int)RequestStatus.Unassigned,
                    Createddate = DateTime.Now,
                };

                var requestClient = new Requestclient
                {
                    Firstname = data.FirstName,
                    Lastname = data.LastName,
                    Email = data.Email,
                    Street = data.Street,
                    City = data.City,
                    State = data.State,
                    Zipcode = data.ZipCode,
                }; 
                
                request.Requestclients.Add(requestClient);
                
                var user = new User
                {
                    Firstname = data.FirstName,
                    Lastname = data.LastName,
                    Mobile = data.PhoneNumber,
                    Email = data.Email,
                    Street = data.Street,
                    City = data.City,
                    State = data.State,
                    Zipcode = data.ZipCode,
                    Createddate = DateTime.Now,
                    Createdby = "patient",
                };

                var file = data.File;
                var uniqueFileName = GetUniqueFileName(file.FileName);
                var uploads = Path.Combine(env.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);
                file.CopyTo(new FileStream(filePath, FileMode.Create));

                var requestWiseFile = new Requestwisefile
                {
                    Createddate = DateTime.Now,
                    Filename = uniqueFileName
                };

                request.Requestwisefiles.Add(requestWiseFile);

                var aspNetUser = new Aspnetuser
                {
                    Aspnetuserid = Guid.NewGuid().ToString(),
                    Username = data.Email,
                    Createddate = DateTime.Now,
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
                var request = new Request
                {
                    Requesttypeid = 3,
                    Firstname = data.FirstName,
                    Lastname = data.LastName,
                    Phonenumber = data.Phone,
                    Email = data.Email,
                    Status = (int)RequestStatus.Unassigned,
                    Createddate = DateTime.Now
                };

                var requestClient = new Requestclient
                {
                    Firstname = data.PatientFirstName,
                    Lastname = data.PatientLastName,
                    Email = data.PatientEmail,
                    Street = data.Street,
                    City = data.City,
                    State = data.State,
                    Zipcode = data.ZipCode,
                };

                request.Requestclients.Add(requestClient);
                
                var file = data.File;
                var uniqueFileName = GetUniqueFileName(file.FileName);
                var uploads = Path.Combine(env.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);
                file.CopyTo(new FileStream(filePath, FileMode.Create));

                var requestWiseFile = new Requestwisefile
                {
                    Createddate = DateTime.Now,
                    Filename = uniqueFileName
                };
                
                request.Requestwisefiles.Add(requestWiseFile);

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
                var request = new Request
                {
                    Requesttypeid = 4,
                    Firstname = data.FirstName,
                    Lastname = data.LastName,
                    Phonenumber = data.Phone,
                    Email = data.Email,
                    Status = (int)RequestStatus.Unassigned,
                    Createddate = DateTime.Now
                };

                var requestClient = new Requestclient
                {
                    Firstname = data.PatientFirstName,
                    Lastname = data.PatientLastName,
                    Email = data.PatientEmail,
                    Street = data.Street,
                    City = data.City,
                    State = data.State,
                    Zipcode = data.ZipCode,
                };

                request.Requestclients.Add(requestClient);

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
                    Ip = "Ip",
                    Concierge = concierge
                };


                request.Requestconcierges.Add(requestConcierge);
                concierge.Requestconcierges.Add(requestConcierge);

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
