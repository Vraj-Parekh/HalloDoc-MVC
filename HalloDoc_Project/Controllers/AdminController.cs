using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Repositories.Repository.Interface;
using Entities.ViewModels;
using MimeKit;
using HalloDoc_Project.Attributes;
using Microsoft.AspNetCore.Authorization;
using HalloDoc.Utility;
using Microsoft.AspNetCore.Authentication;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using Repositories.Utility;
using System.Security.Claims;

namespace HalloDoc_Project.Controllers
{
    [Route("[controller]/[action]")]
    [CustomAuthorization("Admin")]

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
        private readonly IAspNetUserService aspNetUserService;
        private readonly IEncounterFormService encounterFormService;
        private readonly IEmailSender emailSender;
        private readonly IAdminService adminService;

        public IRegionService RegionService { get; }

        public AdminController(IRequestClientServices requestClientServices, IRequestServices requestServices, IRequestNotesServices requestNotesServices, IRequestStatusLogServices requestStatusLogServices, IBlockRequestService blockRequestService, IRegionService regionService, IPhysicianService physicianService, IRequestWiseFilesServices requestWiseFilesServices, IHealthProfessionalTypeService healthProfessionalTypeService, IHealthProfessionalsService healthProfessionalsService, IOrderDetailsService orderDetailsService, IAspNetUserService aspNetUserService, IEncounterFormService encounterFormService, IEmailSender emailSender, IAdminService adminService)
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
            this.aspNetUserService = aspNetUserService;
            this.encounterFormService = encounterFormService;
            this.emailSender = emailSender;
            this.adminService = adminService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult AdminLogin(LoginDTO data)
        {
            string token = aspNetUserService.AuthenticateUser(data);

            if (token != null)
            {
                CookieOptions cookieOptions = new CookieOptions()
                {
                    Expires = DateTime.UtcNow.AddMinutes(60),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                };
                Response.Cookies.Append("Token", token, cookieOptions);

                return RedirectToAction("AdminDashboard", "Admin");
            }
            else
            {
                ModelState.AddModelError(nameof(data.Password), "Incorrect Email or Password.");
                return View(data);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            Response.Cookies.Delete("Token");
            return RedirectToAction("AdminLogin", "Admin");
        }

        [AllowAnonymous]
        public IActionResult ResetPwd()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ResetPwd(LoginDTO data)
        {
            if (aspNetUserService.isUserPresent(data))
            {
                emailSender.SendEmailAsync(data.Email, "Reset Password", $"Tap the link to reset the password: <a href=\"https://localhost:44396/admin/changepassword/{data.Email}\">Reset now</a>");
                return RedirectToAction("AdminLogin", "Admin");
            }
            else
            {
                ModelState.AddModelError(nameof(data.Email), "An account with this email does not exists.");
                return View(data);
            }
        }

        [AllowAnonymous]
        [HttpGet("{email}")]
        public IActionResult ChangePassword(string email)
        {
            ViewBag.email = email;
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(LoginDTO data)
        {
            string email = ViewBag.email;

            if (aspNetUserService.isUserPresent(data))
            {
                aspNetUserService.ChnagePassword(data);
                return RedirectToAction("PatientLogin");
            }

            ModelState.AddModelError(nameof(data.ConfirmPassword), "An account with this email does not exists.");
            return View(data);
        }

        public IActionResult Table(int requestTypeId, int status, int pageIndex, int pageSize)
        {
            int totalCount;
            List<AdminDashboardDTO> data = requestServices.GetPatientdata(requestTypeId, status, pageIndex, pageSize, out totalCount);

            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageIndex = pageIndex;
            return PartialView("_TablePartial", data);
        }

        public IActionResult AdminDashboard()
        {
            object? count = requestServices.GetCount();
            ViewBag.count = count;
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
            ViewBag.requestId = requestId;
            return View(data);
        }

        [HttpPost("{requestId}")]
        public IActionResult ViewNotes(ViewNotesDTO data, int requestId)
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

        [HttpPost("{requestId}")]
        public IActionResult AssignCase(int requestId, string phyRegion, string phyId, string notes)
        {
            requestServices.AssignCase(requestId, phyRegion, phyId, notes);
            return RedirectToAction("AdminDashboard");
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
            Request? request = requestServices.GetRequest(requestId);
            if (request is not null)
            {
                requestWiseFilesServices.AddFiles(files, request);
            }
        }

        public IActionResult Delete(int docId)
        {
            HttpContext? path = HttpContext;
            requestWiseFilesServices.Delete(docId);
            return Redirect(HttpContext.Request.Headers.Referer!);
        }

        [HttpPost]
        public IActionResult DeleteSelectedFiles(List<int> fileIds)
        {
            requestWiseFilesServices.DeleteSelectedFiles(fileIds);
            return Redirect(HttpContext.Request.Headers.Referer!);
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
        public IActionResult SendOrder(SendOrderDTO data, int requestId)
        {
            if (ModelState.IsValid)
            {
                orderDetailsService.AddOrderDetails(data, requestId);
            }
            return View();
        }

        [HttpPost("{requestId}")]
        public IActionResult ClearCase(int requestId)
        {
            requestServices.ClearCase(requestId);
            return RedirectToAction("AdminDashboard");
        }

        [HttpPost("{requestId}")]
        public IActionResult SendAgreement(int requestId, [FromForm] string phoneNumber, [FromForm] string email)
        {
            requestClientServices.SendAgreement(requestId, phoneNumber, email);
            return RedirectToAction("AdminDashboard");
        }

        [HttpGet("{requestId}")]
        public IActionResult Encounter(int requestId)
        {
            ViewBag.requestId = requestId;
            EncounterDTO? data = encounterFormService.GetEncounterInfo(requestId);
            return View(data);
        }

        [HttpPost("{requestId}")]
        public IActionResult Encounter(int requestId, EncounterDTO model)
        {
            encounterFormService.AddEncounterInfo(requestId, model);
            return View();
        }

        [HttpGet("{requestId}")]
        public IActionResult CloseCase(int requestId)
        {
            ViewBag.requestId = requestId;
            ViewDocumentList? data = requestServices.GetCloseCaseInfo(requestId);
            return View(data);
        }

        [HttpPost("{requestId}")]
        public IActionResult CaseClose(int requestId)
        {
            requestServices.AddCloseCaseData(requestId);
            return RedirectToAction("AdminDashboard");
        }

        [HttpPost("{requestId}")]
        public IActionResult UpdateRequestClientMobEmail(int requestId, string email, string phoneNumber)
        {
            requestClientServices.UpdateMobileEmail(requestId, email, phoneNumber);
            return View();
        }


        public async Task<FileResult> ExportFiltered(int requestTypeId, int status, int pageIndex, int pageSize)
        {
            List<Request>? requests = await requestServices.GetFilteredRequests(requestTypeId, status, pageIndex, pageSize);
            byte[]? file = ExcelHelper.CreateFile(requests);
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "patient_list.xlsx");
        }

        public async Task<FileResult> ExportAll(int status)
        {
            List<Request>? requests = await requestServices.GetAllRequests(status);
            byte[]? file = ExcelHelper.CreateFile(requests);
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "all_patient_list.xlsx");
        }
        public IActionResult MyProfile()
        {
            string? email = User.FindFirstValue(ClaimTypes.Email);
            Admin? admin = adminService.GetAdmin(email);
            if (admin is not null && admin.Aspnetuser is not null)
            {
                AdminProfileDTO? model = adminService.GetAdminInfo(admin);
                return View(model);
            }
            return View();
        }

        public IActionResult ResetPassword(AdminProfileDTO model)
        {
            string? email = User.FindFirstValue(ClaimTypes.Email);
            Admin? admin = adminService.GetAdmin(email);
            if(admin is not null && admin.Aspnetuser is not null)
            {
                adminService.ChangePassword(admin, model);
            }
            return RedirectToAction("MyProfile");
        }
        public IActionResult UpdateAdminInfo(AdminProfileDTO model)
        {
            string? email = User.FindFirstValue(ClaimTypes.Email);
            Admin? admin = adminService.GetAdmin(email);
            if (admin is not null)
            {
                adminService.UpdateAdminInfo(admin, model);
            }
            return RedirectToAction("MyProfile");
        }

        public IActionResult UpdateBillingInfo(AdminProfileDTO model)
        {
            string? email = User.FindFirstValue(ClaimTypes.Email);
            Admin? admin = adminService.GetAdmin(email);
            if (admin is not null)
            {
                adminService.UpdateBillingInfo(admin, model);
            }
            return RedirectToAction("MyProfile");
        }

        public IActionResult Provider()
        {
            return View();
        }
    }
}