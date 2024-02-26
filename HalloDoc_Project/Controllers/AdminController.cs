using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Repositories.Repository.Interface;
using Entities.ViewModels;

namespace HalloDoc_Project.Controllers
{
    public class AdminController : Controller
    {
        private readonly IRequestClientServices requestClientServices;
        private readonly IRequestServices requestServices;

        public AdminController(IRequestClientServices requestClientServices,IRequestServices requestServices)
        {
            this.requestClientServices = requestClientServices;
            this.requestServices = requestServices;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AdminDashboard()
        {
            return View();
        }

        [HttpGet("[controller]/[action]/{requestId}")]
        public IActionResult ViewCase(int requestId)
        {
            
            ViewCaseDTO? request = requestServices.GetViewCase(requestId);
            return View(request);
        }

        [HttpPost]
        public IActionResult ViewCase(ViewCaseDTO data)
        {

            if(ModelState.IsValid)
            {
                requestClientServices.UpdateCase(data);
            }
            return View(data);
        }
        public IActionResult ViewNotes()
        {
            return View();
        }
    }
}
