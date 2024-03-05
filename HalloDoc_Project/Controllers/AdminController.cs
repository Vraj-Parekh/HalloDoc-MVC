using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Repositories.Repository.Interface;
using Entities.ViewModels;
using MimeKit;

namespace HalloDoc_Project.Controllers
{
    [Route("[controller]/[action]")]
    public class AdminController : Controller
    {
        private readonly IRequestClientServices requestClientServices;
        private readonly IRequestServices requestServices;
        private readonly IRequestNotesServices requestNotesServices;
        private readonly IRequestStatusLogServices requestStatusLogServices;
        private readonly IBlockRequestService blockRequestService;
        private readonly IRegionService regionService;
        private readonly IPhysicianService physicianService;
        private readonly IRequestWiseFilesServices requestWiseFilesServices;

        public IRegionService RegionService { get; }

        public AdminController(IRequestClientServices requestClientServices, IRequestServices requestServices, IRequestNotesServices requestNotesServices, IRequestStatusLogServices requestStatusLogServices, IBlockRequestService blockRequestService, IRegionService regionService, IPhysicianService physicianService, IRequestWiseFilesServices requestWiseFilesServices)
        {
            this.requestClientServices = requestClientServices;
            this.requestServices = requestServices;
            this.requestNotesServices = requestNotesServices;
            this.requestStatusLogServices = requestStatusLogServices;
            this.blockRequestService = blockRequestService;
            this.regionService = regionService;
            this.physicianService = physicianService;
            this.requestWiseFilesServices = requestWiseFilesServices;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult NewStateTable(int requestTypeId, int status)
        {
            List<AdminDashboardDTO> data = requestServices.GetPatientdata(requestTypeId, 1);
            return PartialView("_NewStateTablePartial", data);
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


        [HttpGet("{requestId}")]
        public IActionResult ViewCase(int requestId)
        {
            ViewCaseDTO? request = requestServices.GetViewCase(requestId);
            return View(request);
        }

        [HttpPost]
        public IActionResult ViewCase(ViewCaseDTO data)
        {
            if (ModelState.IsValid)
            {
                requestClientServices.UpdateCase(data);
                return View(data);
            }
            return View(data);
        }

        [HttpGet("{requestId}")]
        public IActionResult ViewNotes(int requestId)
        {
            ViewNotesDTO? data = requestNotesServices.GetViewRequestNotes(requestId);
            ViewData["RequestId"] = requestId;
            return View(data);
        }

        [HttpPost("{requestId}")]
        public IActionResult ViewNotes(ViewNotesDTO data, [FromRoute] int requestId)
        {
            if (ModelState.IsValid)
            {
                requestNotesServices.AddNotes(data, requestId);
            }
            return View();
        }

        [HttpPost("{requestId}")]
        public IActionResult CancelCase([FromRoute] int requestId, [FromForm] string reason, [FromForm] string notes)
        {
            requestStatusLogServices.AddCancelNote(requestId, reason, notes);
            return RedirectToAction("AdminDashboard");
        }

        public void AssignCase(int requestId, string phyRegion, string phyId, string assignNote)
        {
            requestServices.AssignCase(requestId, phyRegion, phyId, assignNote);
        }


        [HttpPost("{requestId}")]
        public IActionResult BlockCase([FromRoute] int requestId, [FromForm] string blockReason)
        {
            blockRequestService.BlockRequest(requestId, blockReason);
            return RedirectToAction("AdminDashboard");
        }

        public List<Region> FetchRegions()
        {
            List<Region>? regions = regionService.GetRegion();
            return regions;
        }

        [HttpGet("{regionId}")]
        public List<Physician> FetchPhysicianByRegion(int regionId)
        {
            List<Physician>? physician = physicianService.GetPhysician(regionId);
            return physician;
        }

        [HttpGet("{requestId}")]
        public IActionResult ViewUploads(int requestId)
        {
            ViewDocumentList? doc = requestServices.GetDocumentData(requestId);
            return View(doc);
        }

        public IActionResult Download(int docId)
        {
            string filePath = requestWiseFilesServices.GetPath(docId);

            return PhysicalFile(filePath, MimeTypes.GetMimeType(filePath), Path.GetFileName(filePath));
        }

        [HttpPost("{requestId}")]
        public void Upload(int requestId, List<IFormFile> files)
        {
            //Request? request = requestServices.GetRequest(requestId);
            //var result = await requestWiseFileService.AddFilesAsync(files, request);


            //return RedirectToAction("ViewDocument", "Patient", new { requestId });
        }
    }
}