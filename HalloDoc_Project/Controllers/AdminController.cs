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

        [HttpGet]
        public IActionResult NewStateTable(int requestTypeId,int status)
        {
            List<AdminDashboardDTO> data = requestServices.GetPatientdata(requestTypeId,status);   
            return PartialView("_NewStateTablePartial",data);
        }
        public IActionResult PendingStateTable(int requestTypeId, int status)
        {
            List<AdminDashboardDTO> data = requestServices.GetPatientdata(requestTypeId, status);
            return PartialView("_PendingStateTablePartial", data);
        }
        public IActionResult ActiveStateTable(int requestTypeId, int status)
        {
            List<AdminDashboardDTO> data = requestServices.GetPatientdata(requestTypeId, status);
            return PartialView("_ActiveStateTablePartial", data);
        }
        public IActionResult ConcludeStateTable(int requestTypeId, int status)
        {
            List<AdminDashboardDTO> data = requestServices.GetPatientdata(requestTypeId, status);
            return PartialView("_ConcludeStateTablePartial", data);
        }
        public IActionResult ToCloseStateTable(int requestTypeId, int status)
        {
            List<AdminDashboardDTO> data = requestServices.GetPatientdata(requestTypeId, status);
            return PartialView("_ToCloseStateTablePartial", data);
        }
        public IActionResult UnpaidStateTable(int requestTypeId, int status)
        {
            List<AdminDashboardDTO> data = requestServices.GetPatientdata(requestTypeId, status);
            return PartialView("_UnpaidStateTablePartial", data);
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
                return View(data);
            }
            return View(data);
        }

        [HttpGet("[controller]/[action]/{requestId}")]
        public IActionResult ViewNotes()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ViewNotes(ViewNotesDTO data)
        {
            if (ModelState.IsValid)
            {
                return View(data);
            }
            return View(data);
        }

        [HttpGet("[controller]/[action]/{requestId}")]
        public IActionResult CancelCase()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult CancelCase(CancelCaseDTO data)
        {
            if (ModelState.IsValid)
            {
                
                return View(data);
            }
            return View(data);
        }
    }
}
