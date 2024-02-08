using HalloDoc_Project.DTO;
using HalloDoc_Project.Models;
using Microsoft.AspNetCore.Mvc;

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
                    Status = 2
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

                //var user = new User
                //{
                //    Firstname = data.FirstName,
                //    Lastname = data.LastName,
                //    Email = data.Email,
                //    //DateOfBirth = data.DateOfBirth,
                //    State = data.State,
                //    Street = data.Street,
                //    City = data.City,
                //    Zipcode = data.ZipCode,
                //    Createdby = DateTime.Now.ToString()
                //};

     

                try
                {
                    // Add entities to the DbContext and save changes
                    _context.Requestclients.Add(requestClient);
                    _context.Requests.Add(request);
                    _context.SaveChanges();

                    // Redirect to a success page or take appropriate action
                    return RedirectToAction("SubmitRequest", "Request");
                }
                catch (Exception ex)
                {
                    // Log the exception and handle the error appropriately
                    // For example, return an error view
                    ModelState.AddModelError("", "An error occurred while processing your request.");
                    return RedirectToAction("FamilyInfo", "Request");
                }
            }

            // If model state is not valid, return the view with validation errors
            return RedirectToAction("PatientLogin", "Patient");
        }
        public IActionResult FamilyInfo()
        {
            return View();
        }
        public IActionResult ConciergeInfo()
        {
            return View();
        }
        public IActionResult BusinessInfo()
        {
            return View();
        }
    }
}
