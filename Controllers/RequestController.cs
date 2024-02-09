using HalloDoc_Project.DTO;
using HalloDoc_Project.Models;
using Microsoft.AspNetCore.Mvc;
using static Npgsql.PostgresTypes.PostgresCompositeType;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HalloDoc_Project.Controllers
{
    public class RequestController : Controller
    {
        private readonly HalloDocDbContext _context;

        public RequestController(HalloDocDbContext context)
        {
            _context = context;
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
                    Firstname = data.FirstName,
                    Lastname = data.LastName,
                    Email = data.Email,
                    Requesttypeid = 2,
                    Status = 0
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
                
                try
                {
                    _context.Requestclients.Add(requestClient);
                    _context.Requests.Add(request);
                    _context.SaveChanges();
   
                    return RedirectToAction("SubmitRequest", "Request");
                }
                catch (Exception ex)
                {                   
                    ModelState.AddModelError("", "An error occurred while processing your request.");
                    return RedirectToAction("FamilyInfo", "Request");
                }
            }
            return RedirectToAction("PatientLogin", "Patient");
        }
        public IActionResult FamilyInfo()
        {
            return View();
        }
        public IActionResult FamilyInfo(FamilyRequestDTO data)
        {
            
                if (ModelState.IsValid)
                {
                    var request = new Request
                    {
                        Firstname = data.FirstName,
                        Lastname = data.LastName,
                        Email = data.Email,
                        Requesttypeid = 0,
                        Status = 0
                    };

                   

                    int requestId = request.Requestid;

                    var requestClient = new Requestclient
                    {
                        Requestid = requestId,
                        Firstname = data.FirstName,
                        Lastname = data.LastName,
                        Phonenumber = data.Phone,
                        Location = $"{data.City}, {data.State}, {data.ZipCode}",
                        Email = data.Email,
                        Street = data.Street,
                        City = data.City,
                        State = data.State,
                        Zipcode = data.ZipCode,

                    };
                   
                    var requestWiseFile = new Requestwisefile
                    {
                        Requestid = requestId,
                        Filename = "example.txt",
                        Createddate = DateTime.Now,

                    };

                    _context.Requests.Add(request);
                    _context.Requestclients.Add(requestClient);
                    _context.Requestwisefiles.Add(requestWiseFile);
                    _context.SaveChanges();

                    return RedirectToAction("PatientsiteLogin", "Patient");
                }

                return View(data);
            

    }
    public IActionResult ConciergeInfo(ConciergeRequestDTO data)
        {
            if (ModelState.IsValid)
            {
                var request = new Request
                {
                    Firstname = data.PatientFirstName,
                    Lastname = data.PatientLastName,
                    Phonenumber = data.Phone,
                    Email = data.Email,
                    Createddate = DateTime.Now,
                    Status = 0
                };
                _context.Requests.Add(request);

                int requestId = request.Requestid;

                var requestClient = new Requestclient
                {
                    Firstname = data.PatientFirstName,
                    Lastname = data.PatientLastName,
                    Phonenumber = data.Phone,
                    Location = $"{data.City}, {data.State}, {data.ZipCode}",
                    Email = data.Email,
                    Street = data.Street,
                    City = data.City,
                    State = data.State,
                    Zipcode = data.ZipCode
                };
                _context.Requestclients.Add(requestClient);
                request.Requestclients.Add(requestClient);
                var concierge = new Concierge
                {
                    Conciergename = data.FirstName,
                    Street = data.Street,
                    City = data.City,
                    State = data.State,
                    Zipcode = data.ZipCode,
                    Createddate = DateTime.Now,
                    Regionid = 1
                };
                _context.Concierges.Add(concierge);

                int conciergeId = concierge.Conciergeid;

                var requestConcierge = new Requestconcierge
                {

                    Ip = "IPAddress"
                };
                concierge.Requestconcierges.Add(requestConcierge);
                request.Requestconcierges.Add(requestConcierge);
                _context.Requestconcierges.Add(requestConcierge);

                var requestWiseFile = new Requestwisefile
                {

                    Filename = "example.txt",
                    Createddate = DateTime.Now
                };
                //requestWiseFile.Requestid=request.Requestid;
                request.Requestwisefiles.Add(requestWiseFile);
                _context.Requestwisefiles.Add(requestWiseFile);
                _context.SaveChanges();

                return RedirectToAction("PatientsiteLogin", "Patient");
            }
            return View(data);
        }
        public IActionResult BusinessInfo()
        {
            return View();
        }
    }
}
