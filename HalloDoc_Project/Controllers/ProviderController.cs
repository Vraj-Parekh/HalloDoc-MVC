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
using Twilio.Types;

namespace HalloDoc_Project.Controllers
{
    [Route("[controller]/[action]")]
    [CustomAuthorization("Admin, Provider")]
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
                                  IEncounterFormService encounterFormService)
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
        }

        [HttpGet]
        public IActionResult CreateProviderAccount()
        {
            CreatePhysicianDTO? model = new CreatePhysicianDTO();
            model.Regions = regionService.GetRegionList();
            model.Roles = roleService.GetRoles();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProviderAccount(CreatePhysicianDTO model)
        {
            await physicianService.CreatePhysician(model);
            
            return RedirectToAction("UserAccess", "Admin");
        }

        [HttpGet("{physicianId}")]
        public IActionResult EditProviderAccount(int physicianId)
        {
            Physician? physician = physicianService.GetPhysicianById(physicianId);
            if (physician is not null)
            {
                EditPhysicianDTO model = physicianService.GetPhysicianInfo(physician);
                model.Regions = regionService.GetRegionList();
                model.Roles = roleService.GetRoles();

                return View(model);
            }
            return View();
        }

        public async Task<IActionResult> Partners()
        {
            return View();
        }

        public async Task<IActionResult> VendorsTable(string searchVendor,int professionType, int page = 1, int itemsPerPage = 10)
        {
            Pagination<VendorsDTO>? filteredData = await healthProfessionalsService.GetFilteredHealthProfessionals(searchVendor, professionType, page, itemsPerPage);
            return PartialView("_VendorTable", filteredData);
        }

        [HttpGet("{vendorId}")]
        public async Task<IActionResult> EditBusiness(int vendorId)
        {
            EditBusinessDTO? model = await healthProfessionalsService.GetHealthProfessionalInfo(vendorId);
            model.Regions = regionService.GetRegionList();
            model.ProfessionList = healthProfessionalTypeService.GetProfession();

            return View(model);
        }

        [HttpPost("{vendorId}")]
        public async Task<IActionResult> EditBusiness(int vendorId,EditBusinessDTO model)
        {
            await healthProfessionalsService.EditProfessional(vendorId, model);
            return RedirectToAction("Partners","Provider");
        }

        public async Task<IActionResult> AddBusiness()
        {
            EditBusinessDTO? model = new EditBusinessDTO();
            model.Regions = regionService.GetRegionList();
            model.ProfessionList = healthProfessionalTypeService.GetProfession();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddBusiness(EditBusinessDTO model)
        {
            //error : duplicate key violates
            await healthProfessionalsService.AddBusiness(model);
            return RedirectToAction("Partners", "Provider");
        }

        public async Task<IActionResult> PatientHistory()
        {
            return View();
        }

        public async Task<IActionResult> PatientHistoryTable(string firstName, string lastName, string email, string phoneNumber, int page = 1, int itemsPerPage = 10)
        {
            Pagination<PatientHistoryDTO>? filteredData = await userService.GetFilteredUsers(firstName, lastName, email, phoneNumber, page, itemsPerPage);
            return PartialView("_PatientHistoryTable", filteredData);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> PatientRecords(int userId)
        {
            List<PatientRecordsDTO>? modelList = await requestServices.GetPatientRecord(userId);
            return View(modelList);
        }

        public async Task<IActionResult> SearchRecords()
        {
            return View();
        }

        public async Task<IActionResult> SearchRecordsTable(string patientName, string email, string phoneNumber, int requestStatus, int requestType, DateTime fromDateOfService, DateTime toDateOfService, string providerName, int page = 1, int itemsPerPage = 10)
        {
            Pagination<SearchRecordsDTO>? filteredData = await requestServices.GetfilteredSearchRecords(patientName, email, phoneNumber, requestStatus, requestType, fromDateOfService, toDateOfService, providerName,page,itemsPerPage);
            return PartialView("_SearchRecordsTable",filteredData);
        }

        //public async Task<FileResult> ExportSearchRecords(string patientName, string email, string phoneNumber, int requestStatus, int requestType, DateTime fromDateOfService, DateTime toDateOfService, string providerName, int page = 1, int itemsPerPage = 10)
        //{
        //    Pagination<SearchRecordsDTO>? records = await requestServices.GetfilteredSearchRecords(patientName, email, phoneNumber, requestStatus, requestType, fromDateOfService, toDateOfService, providerName, page, itemsPerPage);
        //    byte[]? file = ExcelHelper.CreateFile(records);
        //    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "patient_records.xlsx");
        //}
        [HttpPost("{requestId}")]
        public async Task<IActionResult> DeletePatientRecord(int requestId)
        {
            await requestServices.DeletePatientRecord(requestId);
            return RedirectToAction("SearchRecords", "Provider");
        }

        public async Task<IActionResult> EmailLogs()
        {
            LogsDTO? model = new LogsDTO();
            model.Roles = roleService.GetRoles();
            return View(model);
        }

        public async Task<IActionResult> EmailLogsTable(int role, string receiverName, string emailId, DateTime createdDate, DateTime sentDate, int page = 1, int itemsPerPage = 10)
        {
            Pagination<LogsDTO>? filteredData = await emailLogService.GetFilteredEmailLogs(role, receiverName, emailId, createdDate, sentDate, page, itemsPerPage);
            return PartialView("_EmailLogsTable", filteredData);
        }      
        
        public async Task<IActionResult> SmsLogs()
        {
            LogsDTO? model = new LogsDTO();
            model.Roles = roleService.GetRoles();
            return View(model);
        }

        public async Task<IActionResult> SmsLogsTable(int role, string receiverName, string phoneNumber, DateTime createdDate, DateTime sentDate, int page=1, int itemsPerPage=10)
        {
            Pagination<LogsDTO>? filteredData = await smsLogService.GetFilteredSmsLogs(role, receiverName, phoneNumber, createdDate, sentDate, page, itemsPerPage);
            return PartialView("_SmsLogsTable", filteredData);
        }

        public async Task<IActionResult> BlockHistory()
        {
            return View();
        }

        public async Task<IActionResult> BlockHistoryTable(string name,DateTime createdDate, string email, string phonenumber, int page = 1, int itemsPerPage = 10)
        {
            Pagination<BlockHistoryDTO>? filteredData = await blockRequestService.GetFilteredBlockedHistry(name, createdDate, email, phonenumber, page, itemsPerPage);
            return PartialView("_BlockHistoryTable",filteredData);
        }

        [HttpPost("{requestId}")]
        public async Task<IActionResult> UnblockRequest(int requestId)
        {
            await blockRequestService.UnblockRequest(requestId);
            return RedirectToAction("BlockHistory", "Provider");
        }

        [AllowAnonymous]
        public IActionResult ProviderLogin()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ProviderLogin(LoginDTO data)
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

                return RedirectToAction("ProviderDashboard", "Provider");
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
            return RedirectToAction("ProviderLogin", "Provider");
        }

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

        public IActionResult ProviderDashboard()
        {
            object? count = requestServices.GetCountProvider();
            ViewBag.count = count;
            return View();
        }

        [HttpGet("{requestId}")]
        public async Task<IActionResult> AcceptRequest(int requestId)
        {
            await requestServices.AcceptRequest(requestId);
            return RedirectToAction("ProviderDashboard", "Provider");
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
        public IActionResult CreateRequest()
        {
            return View();
        }

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

            return RedirectToAction("AdminDashboard", "Admin");
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
            return RedirectToAction("ViewNotes", new { requestId = requestId });
        }

        [HttpGet("{requestId}")]
        public IActionResult ViewUploads(int requestId)
        {
            ViewDocumentList? doc = requestServices.GetDocumentData(requestId);
            return View(doc);
        }

        [HttpGet("{requestId}")]
        public IActionResult SendOrder(int requestId)
        {
            ViewBag.requestId = requestId;
            return View();
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

        [HttpPost("{requestId}")]
        public async Task<IActionResult> TransferCase(int requestId,string notes)
        {
            await requestServices.RequestBackToAdmin(requestId, notes);
            return RedirectToAction("ProviderDashboard", "Provider");
        }

        [HttpPost("{requestId}")]
        public IActionResult SendAgreement(int requestId, [FromForm] string phoneNumber, [FromForm] string email)
        {
            requestClientServices.SendAgreement(requestId, phoneNumber, email);
            return RedirectToAction("ProviderDashboard", "Provider");
        }

        [HttpGet("{requestId}")]
        public async Task<IActionResult> HouseCall(int requestId)
        {
            await requestServices.HouseCallStatusChange(requestId);
            return RedirectToAction("ProviderDashboard", "Provider");
        }

        [HttpGet("{requestId}")]
        public async Task<IActionResult> Consult(int requestId)
        {
            await requestServices.ConsultStatusChange(requestId);
            return RedirectToAction("ProviderDashboard", "Provider");
        }
    }
}
