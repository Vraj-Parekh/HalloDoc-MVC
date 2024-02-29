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
        private readonly IRequestNotesServices requestNotesServices;
        private readonly IRequestStatusLogServices requestStatusLogServices;

        public AdminController(IRequestClientServices requestClientServices,IRequestServices requestServices,IRequestNotesServices requestNotesServices,IRequestStatusLogServices requestStatusLogServices)
        {
            this.requestClientServices = requestClientServices;
            this.requestServices = requestServices;
            this.requestNotesServices = requestNotesServices;
            this.requestStatusLogServices = requestStatusLogServices;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult NewStateTable(int requestTypeId,int status)
        {
            List<AdminDashboardDTO> data = requestServices.GetPatientdata(requestTypeId,1);   
            return PartialView("_NewStateTablePartial",data);
        }
        public IActionResult PendingStateTable(int requestTypeId, int status)
        {
            List<AdminDashboardDTO> data = requestServices.GetPatientdata(requestTypeId, 16);
            return PartialView("_PendingStateTablePartial", data);
        }
        public IActionResult ActiveStateTable(int requestTypeId, int status)
        {
#warning Map other status also 
            List<AdminDashboardDTO> data = requestServices.GetPatientdata(requestTypeId, 5);
            return PartialView("_ActiveStateTablePartial", data);
        }
        public IActionResult ConcludeStateTable(int requestTypeId, int status)
        {
            List<AdminDashboardDTO> data = requestServices.GetPatientdata(requestTypeId, 18);
            return PartialView("_ConcludeStateTablePartial", data);
        }
        public IActionResult ToCloseStateTable(int requestTypeId, int status)
        {
#warning Map other status also 
            List<AdminDashboardDTO> data = requestServices.GetPatientdata(requestTypeId, 3);
            return PartialView("_ToCloseStateTablePartial", data);
        }
        public IActionResult UnpaidStateTable(int requestTypeId, int status)
        {
            List<AdminDashboardDTO> data = requestServices.GetPatientdata(requestTypeId, 19);
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
        public IActionResult ViewNotes(int requestId)
        {
            ViewNotesDTO? data = requestNotesServices.GetViewRequestNotes(requestId);
            ViewData["RequestId"] = requestId;
            return View(data);
        }

        [HttpPost("[controller]/[action]/{requestId}")]
        public IActionResult ViewNotes(ViewNotesDTO data,[FromRoute]int requestId)
        {
            if (ModelState.IsValid)
            {
                requestNotesServices.AddNotes(data,requestId);
            }
            return View();
        }

        [HttpPost("[controller]/[action]/{requestId}")]
        public IActionResult CancelCase([FromRoute]int requestId,[FromForm] string reason,[FromForm] string notes)
        {
            requestStatusLogServices.AddCancelNote(requestId, reason, notes);
            return RedirectToAction("AdminDashboard");
        }

    }
}
