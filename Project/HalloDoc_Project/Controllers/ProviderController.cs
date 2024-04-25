using Entities.Models;
using Entities.ViewModels;
using HalloDoc.Utility;
using HalloDoc_Project.Attributes;
using MailKit.Search;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Repository.Implementation;
using Repositories.Repository.Interface;
using Repositories.Utility;
using System.Configuration.Provider;
using System.Drawing;
using System.Security.Claims;
using Twilio.Types;

namespace HalloDoc_Project.Controllers
{
    [Route("[controller]/[action]")]

    public class ProviderController : Controller
    {
        private readonly IRegionService regionService;
        private readonly IRoleService roleService;
        private readonly IPhysicianService physicianService;
        private readonly IShiftService shiftService;
        private readonly IShiftDetailService shiftDetailService;
        private readonly IShiftDetailRegionService shiftDetailRegionService;
        private readonly IHealthProfessionalsService healthProfessionalsService;
        private readonly IHealthProfessionalTypeService healthProfessionalTypeService;
        private readonly IUserService userService;
        private readonly IRequestServices requestServices;
        private readonly IBlockRequestService blockRequestService;
        private readonly IEmailLogService emailLogService;
        private readonly ISmsLogService smsLogService;
        private readonly IAspNetUserService aspNetUserService;
        private readonly IRequestClientServices requestClientServices;
        private readonly IEmailSender emailSender;
        private readonly IRequestNotesServices requestNotesServices;
        private readonly IOrderDetailsService orderDetailsService;
        private readonly IEncounterFormService encounterFormService;
        private readonly IRequestWiseFilesServices requestWiseFilesServices;
        private readonly IPhysicianRegionService physicianRegionService;

        public ProviderController(IRegionService regionService,
                                  IRoleService roleService,
                                  IPhysicianService physicianService,
                                  IShiftService shiftService,
                                  IShiftDetailService shiftDetailService,
                                  IShiftDetailRegionService shiftDetailRegionService,
                                  IHealthProfessionalsService healthProfessionalsService,
                                  IHealthProfessionalTypeService healthProfessionalTypeService,
                                  IUserService userService,
                                  IRequestServices requestServices,
                                  IBlockRequestService blockRequestService,
                                  IEmailLogService emailLogService,
                                  ISmsLogService smsLogService,
                                  IAspNetUserService aspNetUserService,
                                  IRequestClientServices requestClientServices,
                                  IEmailSender emailSender,
                                  IRequestNotesServices requestNotesServices,
                                  IOrderDetailsService orderDetailsService,
                                  IEncounterFormService encounterFormService,
                                  IRequestWiseFilesServices requestWiseFilesServices,
                                  IPhysicianRegionService physicianRegionService)
        {
            this.regionService = regionService;
            this.roleService = roleService;
            this.physicianService = physicianService;
            this.shiftService = shiftService;
            this.shiftDetailService = shiftDetailService;
            this.shiftDetailRegionService = shiftDetailRegionService;
            this.healthProfessionalsService = healthProfessionalsService;
            this.healthProfessionalTypeService = healthProfessionalTypeService;
            this.userService = userService;
            this.requestServices = requestServices;
            this.blockRequestService = blockRequestService;
            this.emailLogService = emailLogService;
            this.smsLogService = smsLogService;
            this.aspNetUserService = aspNetUserService;
            this.requestClientServices = requestClientServices;
            this.emailSender = emailSender;
            this.requestNotesServices = requestNotesServices;
            this.orderDetailsService = orderDetailsService;
            this.encounterFormService = encounterFormService;
            this.requestWiseFilesServices = requestWiseFilesServices;
            this.physicianRegionService = physicianRegionService;
        }

        [CustomAuthorization("Provider Dashboard")]
        public IActionResult Table(int requestTypeId, int status, int pageIndex, int pageSize, string searchQuery)
        {
            int totalCount;
            List<ProviderDashboardDTO> data = requestServices.GetProviderDashboardData(requestTypeId, status, pageIndex, pageSize, searchQuery, out totalCount);

            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageIndex = pageIndex;
            return PartialView("_TablePartial", data);
        }

        [CustomAuthorization("Provider Dashboard")]
        public IActionResult ProviderDashboard()
        {
            object? count = requestServices.GetCountProvider();
            ViewBag.count = count;
            return View();
        }

        [CustomAuthorization("Provider Dashboard")]
        [HttpGet("{requestId}")]
        public async Task<IActionResult> AcceptRequest(int requestId)
        {
            await requestServices.AcceptRequest(requestId);
            TempData["Success"] = "Request Accepted";
            return RedirectToAction("ProviderDashboard", "Provider");
        }

        [CustomAuthorization("Provider RequestData")]
        [HttpGet("{requestId}")]
        public IActionResult ViewCase(int requestId)
        {
            ViewCaseDTO? request = requestServices.GetViewCase(requestId);
            return View(request);
        }

        [CustomAuthorization("Provider RequestData")]
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

        [CustomAuthorization("Provider RequestData")]
        public IActionResult CreateRequest()
        {
            return View();
        }

        [CustomAuthorization("Provider RequestData")]
        [HttpPost]
        public async Task<IActionResult> CreateRequest(CreateRequestDTO model)
        {
            if (!aspNetUserService.isUserEmailPresent(model.Email))
            {
                string subject = "Create your account";
                //string message = "Create Account", $"Tap the link to create account: <a href=\"https://localhost:44396/Patient/createaccount/{request.Requestid}\">Create Now</a>";
                string message = "no idea";

                bool isEmailSent = false;
                try
                {
                    await emailSender.SendEmailAsync(model.Email, subject, message);
                    isEmailSent = true;
                }
                catch (Exception ex)
                {
                }
                await emailLogService.AddEmailLog(model.Email, message, subject, isEmailSent);
            }
            await requestServices.AddRequest(model);

            return RedirectToAction("ProviderDashboard", "Provider");
        }

        [CustomAuthorization("Provider RequestData")]
        [HttpGet("{requestId}")]
        public IActionResult ViewNotes(int requestId)
        {
            ViewNotesDTO? data = requestNotesServices.GetViewRequestNotes(requestId);
            ViewBag.requestId = requestId;
            return View(data);
        }

        [CustomAuthorization("Provider RequestData")]
        [HttpPost("{requestId}")]
        public IActionResult ViewNotes(ViewNotesDTO data, int requestId)
        {
            if (ModelState.IsValid)
            {
                requestNotesServices.AddNotes(data, requestId);
            }
            return RedirectToAction("ViewNotes", new { requestId = requestId });
        }

        [CustomAuthorization("Provider RequestData")]
        [HttpGet("{requestId}")]
        public IActionResult ViewUploads(int requestId)
        {
            ViewDocumentList? doc = requestServices.GetDocumentData(requestId);
            return View(doc);
        }

        [CustomAuthorization("Provider SendOrder")]
        [HttpGet("{requestId}")]
        public IActionResult SendOrder(int requestId)
        {
            ViewBag.requestId = requestId;
            return View();
        }

        [CustomAuthorization("Provider SendOrder")]
        [HttpPost("{requestId}")]
        public IActionResult SendOrder(SendOrderDTO data, int requestId)
        {
            if (ModelState.IsValid)
            {
                orderDetailsService.AddOrderDetails(data, requestId);
            }
            TempData["Success"] = "Order sent successfully";
            return View();
        }

        [CustomAuthorization("Provider RequestData")]
        [HttpGet("{requestId}")]
        public IActionResult Encounter(int requestId)
        {
            ViewBag.requestId = requestId;
            EncounterDTO? data = encounterFormService.GetEncounterInfo(requestId);
            return View(data);
        }

        [CustomAuthorization("Provider RequestData")]
        [HttpGet("{requestId}")]
        public IActionResult EncounterDownload(int requestId)
        {
            ViewBag.requestId = requestId;
            EncounterDTO? data = encounterFormService.GetEncounterInfo(requestId);
            TempData["Success"] = "Form Downloaded successfully";
            return PartialView("Encounter", data);
        }

        [CustomAuthorization("Provider RequestData")]
        [HttpPost("{requestId}")]
        public IActionResult Encounter(int requestId, EncounterDTO model)
        {
            encounterFormService.AddEncounterInfo(requestId, model);
            return View();
        }

        [CustomAuthorization("Provider Dashboard")]
        [HttpGet("{requestId}")]
        public async Task<IActionResult> FinalizeRequest(int requestId)
        {
            await encounterFormService.FinalizeRequest(requestId);
            return RedirectToAction("ProviderDashboard", "Provider");
        }

        [CustomAuthorization("Provider Dashboard")]
        [HttpPost("{requestId}")]
        public async Task<IActionResult> TransferCase(int requestId, string notes)
        {
            await requestServices.RequestBackToAdmin(requestId, notes);
            TempData["Success"] = "Case transfered back to admin";
            return RedirectToAction("ProviderDashboard", "Provider");
        }

        [CustomAuthorization("Provider Dashboard")]
        [HttpPost("{requestId}")]
        public IActionResult SendAgreement(int requestId, [FromForm] string phoneNumber, [FromForm] string email)
        {
            requestClientServices.SendAgreement(requestId, phoneNumber, email);
            TempData["Success"] = "Agreement sent to the patient";
            return RedirectToAction("ProviderDashboard", "Provider");
        }

        [CustomAuthorization("Provider Dashboard")]
        [HttpGet("{requestId}")]
        public async Task<IActionResult> HouseCall(int requestId)
        {
            await requestServices.HouseCallStatusChange(requestId);
            TempData["Success"] = "Calltype HouseCall selected";
            return RedirectToAction("ProviderDashboard", "Provider");
        }

        [CustomAuthorization("Provider Dashboard")]
        [HttpGet("{requestId}")]
        public async Task<IActionResult> Consult(int requestId)
        {
            await requestServices.ConsultStatusChange(requestId);
            TempData["Success"] = "Calltype Consult selected";
            return RedirectToAction("ProviderDashboard", "Provider");
        }

        [CustomAuthorization("Provider Dashboard")]
        [HttpGet("{requestId}")]
        public IActionResult ConcludeCare(int requestId)
        {
            ViewDocumentList? model = requestServices.GetDocumentData(requestId);
            return View(model);
        }

        [CustomAuthorization("Provider Dashboard")]
        [HttpPost("{requestId}")]
        public IActionResult ConcludeCare(int requestId, ViewDocumentList model)
        {
            requestServices.ConcludeService(requestId, model);
            TempData["Success"] = "Request Concluded";
            return RedirectToAction("ProviderDashboard", "Provider");
        }

        [CustomAuthorization("Provider MySchedule")]
        public List<PhysicianRegionsDTO> FetchRegions()
        {
            List<PhysicianRegionsDTO>? regions = physicianRegionService.GetRegions();
            return regions;
        }
        [CustomAuthorization("Provider MySchedule")]
        public IActionResult Scheduling()
        {
            return View();
        }

        [CustomAuthorization("ProviderProfile")]
        public IActionResult MyProfileProvider()
        {
            int physicianId = physicianService.GetPhysicianIdByAspNetUserId(aspNetUserService.GetAspNetUserId());
            Physician? physician = physicianService.GetPhysicianById(physicianId);
            if (physician is not null)
            {
                EditPhysicianDTO model = physicianService.GetPhysicianInfo(physician);
                model.Roles = roleService.GetRoles();

                return View(model);
            }
            return View();
        }

        [CustomAuthorization("Provider Dashboard")]
        public async Task<IActionResult> CheckIsFinalize(int requestId)
        {
            bool finalize = await encounterFormService.isFinalize(requestId);
            return Ok(finalize);
        }

        [CustomAuthorization("Provider Dashboard")]
        public async Task<IActionResult> RequestToAdmin()
        {
            string email = "vrajparekh58@gmail.com";
            string subject = "Request from provider";
            string message = $"Tap the link to submit request: <a href=\"https://localhost:44396/Request/SubmitRequest\">Open</a>";

            bool isEmailSent = false;
            try
            {
                await emailSender.SendEmailAsync(email, subject, message);
                isEmailSent = true;
            }
            catch (Exception ex)
            {
            }
            await emailLogService.AddEmailLog(email, message, subject, isEmailSent);
            if (isEmailSent)
            {
                TempData["Success"] = "Mail sent to admin";
                return RedirectToAction("ProviderDashboard", "Provider");
            }
            TempData["Error"] = "Mail sending fail";
            return RedirectToAction("ProviderDashboard", "Provider");
        }
    }
}
