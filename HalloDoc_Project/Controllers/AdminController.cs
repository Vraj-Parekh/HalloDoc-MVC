﻿using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Repositories.Repository.Interface;
using Entities.ViewModels;
using MimeKit;
using HalloDoc_Project.Attributes;
using Microsoft.AspNetCore.Authorization;
using Org.BouncyCastle.Asn1.Ocsp;
using Repositories.Repository.Implementation;

namespace HalloDoc_Project.Controllers
{
    [Route("[controller]/[action]")]
    //[CustomAuthorization("Admin")]

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
        private readonly IHealthProfessionalTypeService healthProfessionalTypeService;
        private readonly IHealthProfessionalsService healthProfessionalsService;
        private readonly IOrderDetailsService orderDetailsService;

        public IRegionService RegionService { get; }

        public AdminController(IRequestClientServices requestClientServices, IRequestServices requestServices, IRequestNotesServices requestNotesServices, IRequestStatusLogServices requestStatusLogServices, IBlockRequestService blockRequestService, IRegionService regionService, IPhysicianService physicianService, IRequestWiseFilesServices requestWiseFilesServices, IHealthProfessionalTypeService healthProfessionalTypeService,IHealthProfessionalsService healthProfessionalsService,IOrderDetailsService orderDetailsService)
        {
            this.requestClientServices = requestClientServices;
            this.requestServices = requestServices;
            this.requestNotesServices = requestNotesServices;
            this.requestStatusLogServices = requestStatusLogServices;
            this.blockRequestService = blockRequestService;
            this.regionService = regionService;
            this.physicianService = physicianService;
            this.requestWiseFilesServices = requestWiseFilesServices;
            this.healthProfessionalTypeService = healthProfessionalTypeService;
            this.healthProfessionalsService = healthProfessionalsService;
            this.orderDetailsService = orderDetailsService;
        }
        public IActionResult Index()
        {
            return View();
        }

        //[AllowAnonymous]
        //public IActionResult AdminLogin()
        //{
        //    return View();
        //}
        //[HttpPost]
        //[AllowAnonymous]
        //public IActionResult AdminLogin()
        //{

        //}

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

        [HttpPost("{requestId}")]
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

        [HttpGet("{requestId}")]
        public IActionResult SendOrder(int requestId)
        {
            ViewBag.requestId = requestId;
            return View();
        }

        public List<Healthprofessionaltype> FetchProfession()
        {
            return healthProfessionalTypeService.GetProfession();
        }

        [HttpGet("{professionId}")]
        public List<Healthprofessional> FetchBusiness(int professionId)
        {
            return healthProfessionalsService.GetBusiness(professionId);
        }

        [HttpPost("{requestId}")]
        public IActionResult SendOrder(SendOrderDTO data,int requestId)
        {
            if(ModelState.IsValid)
            {
                orderDetailsService.AddOrderDetails(data, requestId);
            }
            return View();
        }
        

    }
}