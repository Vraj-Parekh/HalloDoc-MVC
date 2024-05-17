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
using NPOI.OpenXmlFormats.Vml;
using System.Globalization;
using Repositories.Repository.Implementation;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities;
using MailKit.Search;
using System.Xml.Schema;
using System.Security.Cryptography.X509Certificates;
using System.IdentityModel.Tokens.Jwt;
using static ICSharpCode.SharpZipLib.Zip.ExtendedUnixData;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
        private readonly IHealthProfessionalTypeService healthProfessionalTypeService;
        private readonly IHealthProfessionalsService healthProfessionalsService;
        private readonly IOrderDetailsService orderDetailsService;
        private readonly IAspNetUserService aspNetUserService;
        private readonly IEncounterFormService encounterFormService;
        private readonly IEmailSender emailSender;
        private readonly IAdminService adminService;
        private readonly ISmsSender smsSender;
        private readonly IMenuService menuService;
        private readonly IRoleService roleService;
        private readonly IRoleMenuService roleMenuService;
        private readonly IAdminRegionService adminRegionService;
        private readonly IShiftDetailService shiftDetailService;
        private readonly IShiftService shiftService;
        private readonly IShiftDetailRegionService shiftDetailRegionService;
        private readonly IEmailLogService emailLogService;
        private readonly ISmsLogService smsLogService;
        private readonly IPhysicianLocationService physicianLocationService;
        private readonly IHttpContextAccessor httpContext;
        private readonly IHelperService helperService;
        private readonly IUserService userService;
        private readonly IHandleShiftService handleShiftService;
        private readonly IHandlePhysicianService handlePhysicianService;
        private readonly ITimesheetService timesheetService;
        private readonly IPayrateService payrateService;

        public AdminController(IRequestClientServices requestClientServices,
                               IRequestServices requestServices,
                               IRequestNotesServices requestNotesServices,
                               IRequestStatusLogServices requestStatusLogServices,
                               IBlockRequestService blockRequestService,
                               IRegionService regionService,
                               IPhysicianService physicianService,
                               IRequestWiseFilesServices requestWiseFilesServices,
                               IHealthProfessionalTypeService healthProfessionalTypeService,
                               IHealthProfessionalsService healthProfessionalsService,
                               IOrderDetailsService orderDetailsService,
                               IAspNetUserService aspNetUserService,
                               IEncounterFormService encounterFormService,
                               IEmailSender emailSender,
                               IAdminService adminService,
                               ISmsSender smsSender,
                               IMenuService menuService,
                               IRoleService roleService,
                               IRoleMenuService roleMenuService,
                               IAdminRegionService adminRegionService,
                               IShiftDetailService shiftDetailService,
                               IShiftService shiftService,
                               IShiftDetailRegionService shiftDetailRegionService,
                               IEmailLogService emailLogService,
                               ISmsLogService smsLogService,
                               IPhysicianLocationService physicianLocationService,
                               IHttpContextAccessor httpContext,
                               IHelperService helperService,
                               IUserService userService,
                               IHandleShiftService handleShiftService,
                               IHandlePhysicianService handlePhysicianService,
                               ITimesheetService timesheetService,
                               IPayrateService payrateService)
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
            this.smsSender = smsSender;
            this.menuService = menuService;
            this.roleService = roleService;
            this.roleMenuService = roleMenuService;
            this.adminRegionService = adminRegionService;
            this.shiftDetailService = shiftDetailService;
            this.shiftService = shiftService;
            this.shiftDetailRegionService = shiftDetailRegionService;
            this.emailLogService = emailLogService;
            this.smsLogService = smsLogService;
            this.physicianLocationService = physicianLocationService;
            this.httpContext = httpContext;
            this.helperService = helperService;
            this.userService = userService;
            this.handleShiftService = handleShiftService;
            this.handlePhysicianService = handlePhysicianService;
            this.timesheetService = timesheetService;
            this.payrateService = payrateService;
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

                JwtService.ValidateToken(token, out JwtSecurityToken jwtSecurityToken);

                var roleClaim = jwtSecurityToken.Claims.Where(claims => claims.Type == ClaimTypes.Role).Select(a => a.Value).ToList();
                if (roleClaim.Contains("Admin"))
                {
                    TempData["Success"] = "Login successful";
                    return RedirectToAction("AdminDashboard", "Admin");
                }
                TempData["Success"] = "Login successful";
                return RedirectToAction("ProviderDashboard", "Provider");
            }
            else
            {
                TempData["Error"] = "Invalid credentials";
                ModelState.AddModelError(nameof(data.Password), "Incorrect Email or Password.");
                return View(data);
            }
        }

        [AllowAnonymous]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            Response.Cookies.Delete("Token");
            return RedirectToAction("AdminLogin", "Admin");
        }

        [CustomAuthorization("Dashboard")]
        public IActionResult Table(int requestTypeId, int status, int pageIndex, int pageSize, string searchQuery, int regionId)
        {
            int totalCount;
            List<AdminDashboardDTO> data = requestServices.GetPatientdata(requestTypeId, status, pageIndex, pageSize, searchQuery, regionId, out totalCount);

            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageIndex = pageIndex;
            return PartialView("_TablePartial", data);
        }

        [CustomAuthorization("Dashboard")]
        public IActionResult AdminDashboard()
        {
            object? count = requestServices.GetCount();
            ViewBag.count = count;
            return View();
        }

        [CustomAuthorization("Dashboard")]
        public async Task<IActionResult> SendLink(SendLinkDTO model)
        {
            var subject = "Admin sent you link";
            var message = "<a href=\"https://localhost:44396/Request/SubmitRequest\">Open request page</a>";
            bool isEmailSent = false;
            try
            {
                await emailSender.SendEmailAsync(model.Email, subject, message);
                isEmailSent = true;
                TempData["Success"] = "Mail sent successful";
            }
            catch (Exception e)
            {
                await emailLogService.AddEmailLog(model.Email, message, subject, isEmailSent);
                TempData["Error"] = "Error in sending mail";
            }
            await emailLogService.AddEmailLog(model.Email, message, subject, isEmailSent);
            return RedirectToAction("AdminDashboard", "Admin");
        }

        [CustomAuthorization("RequestData")]
        [HttpGet("{requestId}")]
        public IActionResult ViewCase(int requestId)
        {
            ViewCaseDTO? request = requestServices.GetViewCase(requestId);
            return View(request);
        }

        [CustomAuthorization("RequestData")]
        [HttpPost("{requestId}")]
        public IActionResult ViewCase(ViewCaseDTO data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    requestClientServices.UpdateCase(data);
                    TempData["Success"] = "Data updated";
                }
                catch (Exception e)
                {
                    TempData["Error"] = "Error in data updation";
                }
            }
            return View(data);
        }

        [CustomAuthorization("RequestData")]
        [HttpGet("{requestId}")]
        public IActionResult ViewNotes(int requestId)
        {
            ViewNotesDTO? data = requestNotesServices.GetViewRequestNotes(requestId);
            ViewBag.requestId = requestId;
            return View(data);
        }

        [CustomAuthorization("RequestData")]
        [HttpPost("{requestId}")]
        public IActionResult ViewNotes(ViewNotesDTO data, int requestId)
        {
            if (ModelState.IsValid)
            {
                requestNotesServices.AddNotes(data, requestId);
            }
            return RedirectToAction("ViewNotes", new { requestId = requestId });
        }

        [CustomAuthorization("RequestData")]
        [HttpPost("{requestId}")]
        public IActionResult CancelCase([FromRoute] int requestId, [FromForm] string reason, [FromForm] string notes)
        {
            try
            {
                requestStatusLogServices.AddCancelNote(requestId, reason, notes);
                TempData["Success"] = "Case Cancelled";
            }
            catch (Exception e)
            {
                TempData["Error"] = "Error in cancelling the case";
            }
            return RedirectToAction("AdminDashboard");
        }

        [CustomAuthorization("RequestData")]
        [HttpPost("{requestId}")]
        public IActionResult AssignCase(int requestId, string phyRegion, string phyId, string notes)
        {
            //status-->reamil to accept -> 23
            try
            {
                requestServices.AssignCase(requestId, phyRegion, phyId, notes);
                TempData["Success"] = "Case assigned to physician";
            }
            catch (Exception e)
            {
                TempData["Error"] = "Error in assigning the case";
            }
            return RedirectToAction("AdminDashboard");
        }

        [CustomAuthorization("RequestData")]
        [HttpPost("{requestId}")]
        public IActionResult BlockCase([FromRoute] int requestId, [FromForm] string blockReason)
        {
            try
            {
                blockRequestService.BlockRequest(requestId, blockReason);
                TempData["Success"] = "Request Blocked";
            }
            catch (Exception e)
            {
                TempData["Error"] = "Error in blocking request";
            }
            return RedirectToAction("AdminDashboard");
        }

        [CustomAuthorization("RequestData")]
        public List<Region> FetchRegions()
        {
            List<Region>? regions = regionService.GetRegion();
            return regions;
        }

        [CustomAuthorization("RequestData,Provider MySchedule")]
        [HttpGet("{regionId}")]
        public List<Physician> FetchPhysicianByRegion(int regionId)
        {
            List<Physician>? physician = physicianService.GetPhysicianByRegionId(regionId);
            return physician;
        }

        [CustomAuthorization("RequestData")]
        [HttpGet("{requestId}")]
        public IActionResult ViewUploads(int requestId)
        {
            ViewDocumentList? doc = requestServices.GetDocumentData(requestId);
            return View(doc);
        }

        [CustomAuthorization("RequestData,Provider RequestData,Patient")]
        public IActionResult Download(int docId)
        {
            string filePath = requestWiseFilesServices.GetPath(docId);

            return PhysicalFile(filePath, MimeTypes.GetMimeType(filePath), Path.GetFileName(filePath));
        }

        [CustomAuthorization("RequestData,Provider RequestData,Patient")]
        [HttpPost("{requestId}")]
        public void Upload(int requestId, List<IFormFile> files)
        {
            Request? request = requestServices.GetRequest(requestId);
            if (request is not null)
            {
                requestWiseFilesServices.AddFiles(files, request);
            }
        }

        [CustomAuthorization("RequestData,Provider RequestData,Patient")]
        public IActionResult Delete(int docId)
        {
            HttpContext? path = HttpContext;
            requestWiseFilesServices.Delete(docId);
            return Redirect(HttpContext.Request.Headers.Referer!);
        }

        [CustomAuthorization("RequestData,Provider RequestData,Patient")]
        [HttpPost]
        public IActionResult DeleteSelectedFiles(List<int> fileIds)
        {
            requestWiseFilesServices.DeleteSelectedFiles(fileIds);
            return Redirect(HttpContext.Request.Headers.Referer!);
        }

        [CustomAuthorization("RequestData")]
        [HttpPost]
        public async Task SendAttachment(int request_id, int[] files_jx, string mail)
        {
            try
            {
                await helperService.SendAttachment(request_id, files_jx, mail);
                TempData["Success"] = "Attachment sent to the patient";
            }
            catch (Exception e)
            {
                TempData["Error"] = "Error in sending the attachment";
            }
        }

        [CustomAuthorization("SendOrder")]
        [HttpGet("{requestId}")]
        public IActionResult SendOrder(int requestId)
        {
            ViewBag.requestId = requestId;
            return View();
        }

        [CustomAuthorization("RequestData,Provider RequestData")]
        public List<Healthprofessionaltype> FetchProfession()
        {
            return healthProfessionalTypeService.GetProfession();
        }

        [CustomAuthorization("RequestData,Provider RequestData")]
        [HttpGet("{professionId}")]
        public List<Healthprofessional> FetchBusiness(int professionId)
        {
            return healthProfessionalsService.GetBusiness(professionId);
        }

        [CustomAuthorization("SendOrder")]
        [HttpPost("{requestId}")]
        public IActionResult SendOrder(SendOrderDTO data, int requestId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    orderDetailsService.AddOrderDetails(data, requestId);
                    TempData["Success"] = "Order sent successfully";
                }
                catch (Exception e)
                {
                    TempData["Error"] = "Error in sending order";
                }
            }
            return View();
        }

        [CustomAuthorization("RequestData")]
        [HttpPost("{requestId}")]
        public IActionResult ClearCase(int requestId)
        {
            try
            {
                requestServices.ClearCase(requestId);
                TempData["Success"] = "Case cleared successfully";
            }
            catch (Exception e)
            {
                TempData["Error"] = "Error in clear case";
            }
            return RedirectToAction("AdminDashboard");
        }

        [CustomAuthorization("RequestData")]
        [HttpPost("{requestId}")]
        public IActionResult SendAgreement(int requestId, [FromForm] string phoneNumber, [FromForm] string email)
        {
            try
            {
                requestClientServices.SendAgreement(requestId, phoneNumber, email);
                TempData["Success"] = "Agrrement sent to the patient";
            }
            catch
            {
                TempData["Error"] = "Error in sending the agreement";
            }
            return RedirectToAction("AdminDashboard");
        }

        [CustomAuthorization("RequestData")]
        [HttpGet("{requestId}")]
        public IActionResult Encounter(int requestId)
        {
            ViewBag.requestId = requestId;
            EncounterDTO? data = encounterFormService.GetEncounterInfo(requestId);
            return View(data);
        }

        [CustomAuthorization("RequestData")]
        [HttpPost("{requestId}")]
        public IActionResult Encounter(int requestId, EncounterDTO model)
        {
            try
            {
                encounterFormService.AddEncounterInfo(requestId, model);
                TempData["Success"] = "Encounter details added";
            }
            catch (Exception e)
            {
                TempData["Error"] = "Error in adding details";
            }
            return View();
        }

        [CustomAuthorization("RequestData")]
        [HttpGet("{requestId}")]
        public IActionResult CloseCase(int requestId)
        {
            ViewBag.requestId = requestId;
            ViewDocumentList? data = requestServices.GetCloseCaseInfo(requestId);
            return View(data);
        }

        [CustomAuthorization("RequestData")]
        [HttpPost("{requestId}")]
        public IActionResult CaseClose(int requestId)
        {
            try
            {
                requestServices.AddCloseCaseData(requestId);
                TempData["Success"] = "Case closed";
            }
            catch (Exception e)
            {
                TempData["Error"] = "Error in case close";
            }
            return RedirectToAction("AdminDashboard");
        }

        [CustomAuthorization("RequestData")]
        [HttpPost("{requestId}")]
        public IActionResult UpdateRequestClientMobEmail(int requestId, string email, string phoneNumber)
        {
            try
            {
                requestClientServices.UpdateMobileEmail(requestId, email, phoneNumber);
                TempData["Success"] = "Details updated";
            }
            catch (Exception e)
            {
                TempData["Error"] = "Error in updating details";
            }
            return View();
        }

        private DateTime GenerateDateOfBirth(int? year, string? month, int? date)
        {
            DateTime finalDate = new DateTime(year ?? 1900, DateTime.ParseExact(month ?? "January", "MMMM", CultureInfo.CurrentCulture).Month, date ?? 01);
            return finalDate;
        }

        [CustomAuthorization("Dashboard")]
        public FileResult ExportFiltered(int requestTypeId, int status, int pageIndex, int pageSize, string searchQuery, int regionId)
        {
            try
            {
                int totalCount;
                List<AdminDashboardDTO>? requests = requestServices.GetPatientdata(requestTypeId, status, pageIndex, pageSize, searchQuery, regionId, out totalCount);
                byte[]? file = ExcelHelper.CreateFile(requests);
                TempData["Success"] = "Data exported successfully";
                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "patient_list.xlsx");
            }
            catch (Exception e)
            {
                TempData["Error"] = "Error in exporting";
            }
            return null;
        }

        [CustomAuthorization("Dashboard")]
        public async Task<FileResult> ExportAll(int status)
        {
            try
            {
                List<Request>? requests = await requestServices.GetAllRequests(status);
                byte[]? file = ExcelHelper.CreateFile(requests);
                TempData["Success"] = "Data exported successfully";
                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "all_patient_list.xlsx");
            }
            catch (Exception e)
            {
                TempData["Error"] = "Error in exporting";
            }
            return null;
        }

        [CustomAuthorization("Dashboard,Provider Dashboard")]
        public IActionResult CreateRequest()
        {
            return View();
        }

        [CustomAuthorization("Dashboard,Provider Dashboard")]
        [HttpPost]
        public async Task<IActionResult> CreateRequest(CreateRequestDTO model)
        {
            if (!aspNetUserService.isUserEmailPresent(model.Email))
            {
                string subject = "Create your account";
                string message = $"Tap the link to submit request: <a href=\"https://localhost:44396/Request/SubmitRequest\">Open</a>";
                bool isEmailSent = false;
                try
                {
                    await emailSender.SendEmailAsync(model.Email, subject, message);
                    isEmailSent = true;
                }
                catch (Exception ex)
                {
                    await emailLogService.AddEmailLog(model.Email, message, subject, isEmailSent);
                }
                await emailLogService.AddEmailLog(model.Email, message, subject, isEmailSent);
            }
            await requestServices.AddRequest(model);

            return RedirectToAction("AdminDashboard", "Admin");
        }

        [CustomAuthorization("MyProfile")]
        public IActionResult MyProfile()
        {
            string? email = User.FindFirstValue(ClaimTypes.Email);
            Admin? admin = adminService.GetAdmin(email);
            if (admin is not null && admin.Aspnetuser is not null)
            {
                AdminProfileDTO? model = adminService.GetAdminInfo(admin);
                model.Roles = roleService.GetRoles();

                return View(model);
            }
            return View();
        }

        [CustomAuthorization("MyProfile")]
        public async Task<IActionResult> ResetPasswordAsync(AdminProfileDTO model)
        {
            string? email = User.FindFirstValue(ClaimTypes.Email);
            Admin? admin = adminService.GetAdmin(email);
            if (admin is not null && admin.Aspnetuser is not null)
            {
                try
                {
                    await adminService.ChangePassword(admin, model);
                    TempData["Success"] = "Data Updated";
                }
                catch (Exception e)
                {
                    TempData["Errror"] = "Error in data updation";
                }
            }
            return RedirectToAction("MyProfile");
        }

        [CustomAuthorization("MyProfile")]
        public async Task<IActionResult> UpdateAdminInfoAsync(AdminProfileDTO model)
        {
            string? email = User.FindFirstValue(ClaimTypes.Email);
            Admin? admin = adminService.GetAdmin(email);

            if (admin is not null)
            {
                try
                {
                    if (admin.Email != model.Email)
                    {
                        if (await helperService.IsAspNetUserEmailPresent(model.Email))
                        {
                            TempData["Error"] = "Email already present";
                            return RedirectToAction("MyProfile");
                        }
                        return RedirectToAction("Logout");
                    }
                    await adminService.UpdateAdminInfo(admin, model);
                    TempData["Success"] = "Data Updated";
                }
                catch (Exception e)
                {
                    TempData["Error"] = "Error in data updation";
                }
            }
            return RedirectToAction("MyProfile");
        }

        [CustomAuthorization("MyProfile")]
        public async Task<IActionResult> UpdateBillingInfoAsync(AdminProfileDTO model)
        {
            string? email = User.FindFirstValue(ClaimTypes.Email);
            Admin? admin = adminService.GetAdmin(email);
            if (admin is not null)
            {
                await adminService.UpdateBillingInfo(admin, model);
            }
            return RedirectToAction("MyProfile");
        }


        [CustomAuthorization("Accounts")]
        [HttpGet("{id}")]
        public IActionResult EditAdmin(int id)
        {
            Admin? admin = adminService.GetAdminById(id);
            if (admin is not null)
            {
                AdminProfileDTO? model = adminService.GetAdminInfo(admin);
                model.Roles = roleService.GetRoles();

                return View(model);
            }
            return View();
        }

        [CustomAuthorization("Accounts")]
        public async Task<IActionResult> ResetPasswordEditAsync(AdminProfileDTO model)
        {
            Admin? admin = adminService.GetAdminById(model.AdminId);
            if (admin is not null)
            {
                try
                {
                    await adminService.ChangePassword(admin, model);
                    TempData["Success"] = "Data Updated";
                }
                catch (Exception e)
                {
                    TempData["Error"] = "Error in data updation";
                }
            }
            return RedirectToAction("EditAdmin", new { id = model.AdminId });
        }

        [CustomAuthorization("Accounts")]
        public async Task<IActionResult> UpdateAdminInfoEditAsync(AdminProfileDTO model)
        {
            Admin? admin = adminService.GetAdminById(model.AdminId);
            if (admin is not null)
            {
                try
                {
                    await adminService.UpdateAdminInfo(admin, model);
                    TempData["Success"] = "Data Updated";
                }
                catch (Exception e)
                {
                    TempData["Error"] = "Error in data updation";
                }
            }
            return RedirectToAction("EditAdmin", new { id = model.AdminId });
        }

        [CustomAuthorization("Accounts")]
        public async Task<IActionResult> UpdateBillingInfoEditAsync(AdminProfileDTO model)
        {
            Admin? admin = adminService.GetAdminById(model.AdminId);
            if (admin is not null)
            {
                try
                {
                    await adminService.UpdateBillingInfo(admin, model);
                    TempData["Success"] = "Data Updated";
                }
                catch (Exception e)
                {
                    TempData["Error"] = "Error in data updation";
                }
            }
            return RedirectToAction("EditAdmin", new { id = model.AdminId });
        }

        [CustomAuthorization("Provider")]
        public IActionResult Providers()
        {
            return View();
        }

        [CustomAuthorization("Provider")]
        public async Task<IActionResult> ProvidersTable(int regionId, int page = 1, int itemsPerPage = 5)
        {
            Pagination<ProviderMenuDTO>? filteredData = await physicianService.GetProviderMenu(regionId, page, itemsPerPage);
            return PartialView("_ProvidersTable", filteredData);
        }

        [CustomAuthorization("Provider")]
        public async Task SendMessage(int mode, string message, int physicianId)
        {
            string email = physicianService.GetPhysicianEmail(physicianId);
            string phoneNumber = physicianService.GetPhysicianPhone(physicianId);
            string subject = "For communication";
            bool isSmsSent = false;
            bool isEmailSent = false;

            //message-1
            if (mode == 1)
            {
                try
                {
                    smsSender.SendSms(phoneNumber, message);
                    isSmsSent = true;
                    TempData["Success"] = "Sms sent successfully";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error in sending sms";
                }
                await smsLogService.AddSmsLog(phoneNumber, message, isSmsSent);
            }
            //email-2
            else if (mode == 2)
            {
                try
                {
                    await emailSender.SendEmailAsync(email, subject, message);
                    isEmailSent = true;
                    TempData["Success"] = "Mail sent successfully";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error in sending mail";
                }
                await emailLogService.AddEmailLog(email, message, subject, isEmailSent);
            }
            //both-0
            else
            {

                try
                {
                    smsSender.SendSms(phoneNumber, message);
                    isSmsSent = true;
                    TempData["Success"] = "Sms sent successfully";
                }
                catch (Exception e)
                {
                    TempData["Error"] = "Error in sending sms";
                }
                try
                {
                    await emailSender.SendEmailAsync(email, subject, message);
                    isEmailSent = true;
                    TempData["Success"] = "Email sent successfully";
                }
                catch (Exception e)
                {
                    TempData["Error"] = "Error in sending mail";
                }
                // Log SMS and email sending status
                await smsLogService.AddSmsLog(phoneNumber, message, isSmsSent);
                await emailLogService.AddEmailLog(email, message, subject, isEmailSent);
            }
        }

        [CustomAuthorization("Accounts")]
        public IActionResult CreateRole()
        {
            return View();
        }

        [CustomAuthorization("Accounts")]
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleDTO model)
        {
            if (!roleService.IsRolePresent(model.RoleName))
            {
                try
                {
                    await roleService.AddRole(model);
                    TempData["Success"] = "New role added successfully";
                }
                catch (Exception e)
                {
                    TempData["Error"] = "Error in creating role";
                }
                return Ok();
            }
            else
            {
                return BadRequest(new { message = "Role Name Exist" });
            }
        }

        [CustomAuthorization("Accounts")]
        [HttpGet("{roleId}")]
        public IActionResult EditRole(int roleId)
        {
            CreateRoleDTO? role = roleService.GetRoleById(roleId);
            return View(role);
        }

        [CustomAuthorization("Accounts")]
        [HttpPost("{roleId}")]
        public async Task<IActionResult> EditRole(int roleId, CreateRoleDTO model)
        {
            try
            {
                await roleService.EditRole(roleId, model);
                TempData["Success"] = "Role edited successfully";
            }
            catch (Exception e)
            {
                TempData["Error"] = "Error in editing the role";
            }
            return RedirectToAction("AccountAccess");
        }

        [CustomAuthorization("Accounts")]
        [HttpPost("{roleId}")]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            try
            {
                await roleService.DeleteRole(roleId);
                TempData["Success"] = "Role deleted successfully";
            }
            catch (Exception e)
            {
                TempData["Success"] = "Error in deleting the role";
            }
            return RedirectToAction("AccountAccess");
        }

        [CustomAuthorization("Accounts")]
        public List<Menu> FetchMenus(int accountType)
        {
            List<Menu>? menus = menuService.GetMenus(accountType);
            return menus;
        }

        [CustomAuthorization("Accounts")]
        public IActionResult AccountAccess()
        {
            List<AccountAccessDTO>? data = roleService.GetAllRoles();
            return View(data);
        }

        [CustomAuthorization("Accounts")]
        public async Task<IActionResult> UserAccess()
        {
            return View();
        }

        [CustomAuthorization("Accounts")]
        public async Task<IActionResult> UserAccessTable(int accountType, int page = 1, int itemsPerPage = 5)
        {
            Pagination<UserAccessDTO>? filteredData = await adminService.GetFilteredUserAccessData(accountType, page, itemsPerPage);
            return PartialView("_UserAccessTable", filteredData);
        }

        [CustomAuthorization("Manage Admin")]
        public IActionResult CreateAdminAccount()
        {
            CreateAdminDTO model = new CreateAdminDTO();
            model.Regions = regionService.GetRegionList();
            model.Roles = roleService.GetRoles();

            return View(model);
        }

        [CustomAuthorization("Manage Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAdminAccount(CreateAdminDTO model)
        {
            try
            {
                if (await helperService.IsAspNetUserEmailPresent(model.Email))
                {
                    ModelState.AddModelError(nameof(model.Email), "Email Already Present");
                    model.Regions = regionService.GetRegionList();
                    model.Roles = roleService.GetRoles();
                    return View(model);
                }
                await adminService.CreateAdmin(model);
                TempData["Success"] = "Account Created";
            }
            catch (Exception e)
            {
                TempData["Error"] = "Error in creating the account";
            }
            return RedirectToAction("UserAccess", "Admin");
        }

        [CustomAuthorization("Scheduling,Provider MySchedule")]
        public IActionResult FetchPhysician(int regionId = 0)
        {
            List<Physician>? physician = physicianService.GetPhysicianByRegionId(regionId);
            return Json(physician);
        }

        [CustomAuthorization("Scheduling,Provider MySchedule")]
        private async Task<List<Shiftdetail>> GetAndPrepareShiftDetails()
        {
            var shiftDetails = await shiftDetailService.GetShiftDetails();
            foreach (var shift in shiftDetails)
            {
                shift.Shift.Shiftdetails = null;
                shift.Shift.Physician.Shifts = null;
            }
            return shiftDetails;
        }

        [CustomAuthorization("Scheduling,Provider MySchedule")]
        public async Task<IActionResult> FetchShiftDetails()
        {
            var shiftDetails = await GetAndPrepareShiftDetails();
            return Json(shiftDetails);
        }

        [CustomAuthorization("Scheduling")]
        public async Task<IActionResult> ReturnShift(int shiftDetailId)
        {
            await shiftDetailService.ChangeShiftStatus(shiftDetailId);
            return await FetchShiftDetails();
        }

        [CustomAuthorization("Scheduling")]
        public async Task<IActionResult> DeleteShift(int shiftDetailId)
        {
            await shiftDetailService.DeleteShift(shiftDetailId);
            return await FetchShiftDetails();
        }

        [CustomAuthorization("Scheduling")]
        public async Task<IActionResult> DeleteSelectedShift(List<int> Ids)
        {
            await shiftDetailService.DeleteSelectedShift(Ids);
            return RedirectToAction("RequestedShift", "Admin");
        }

        [CustomAuthorization("Scheduling,Provider MySchedule")]
        public async Task<IActionResult> SaveShift(ScheduleDTO model)
        {
            if (await shiftDetailService.EditShift(model))
            {
                var res = await FetchShiftDetails();
                return res;
            }
            return BadRequest("Can't edit shift");
        }

        [CustomAuthorization("Scheduling")]
        public async Task<IActionResult> ApproveSelected(List<int> Ids)
        {
            await shiftDetailService.ApproveShift(Ids);
            return RedirectToAction("RequestedShift", "Admin");
        }

        [CustomAuthorization("Scheduling,Provider MySchedule")]
        public async Task<IActionResult> CreateShift(CreateShiftDTO model)
        {
            try
            {
                await handleShiftService.AddShiftDetails(model);
                var res = await FetchShiftDetails();
                return Ok(res);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Shift already present";
            }
            return BadRequest("Shift not created");
        }

        [CustomAuthorization("Scheduling")]
        public IActionResult Scheduling()
        {
            return View();
        }

        [CustomAuthorization("Scheduling")]
        public IActionResult ProviderOnCall()
        {
            ScheduleDTO model = new ScheduleDTO();
            model.Regions = regionService.GetRegionList();
            return View(model);
        }

        [CustomAuthorization("Scheduling")]
        public async Task<IActionResult> MdOnCallDiv(int regionId)
        {
            MdOncallDTO? filteredData = await shiftDetailService.GetOnCallData(regionId);
            return PartialView("_mdOnCall", filteredData);
        }

        [CustomAuthorization("Scheduling")]
        public async Task<IActionResult> RequestedShift()
        {
            ScheduleDTO model = new ScheduleDTO();
            model.Regions = regionService.GetRegionList();
            return View(model);
        }

        [CustomAuthorization("Scheduling")]
        public async Task<IActionResult> RequestedShiftTable(int regionId, bool isDateFilter, int page = 1, int itemsPerPage = 10)
        {
            Pagination<RequestedShiftDTO>? filteredData = await shiftService.GetFilteredRequestedShifts(regionId, isDateFilter, page, itemsPerPage);
            return PartialView("_RequestedShiftTable", filteredData);
        }

        [CustomAuthorization("ProviderLocation")]
        public IActionResult ProviderLocation()
        {
            List<Physicianlocation>? location = physicianLocationService.GetLocation();
            return View(location);
        }

        [CustomAuthorization("Provider")]
        [HttpGet]
        public IActionResult CreateProviderAccount()
        {
            CreatePhysicianDTO? model = new CreatePhysicianDTO();
            model.Regions = regionService.GetRegionList();
            model.Roles = roleService.GetRoles();

            return View(model);
        }

        [CustomAuthorization("Provider")]
        [HttpPost]
        public async Task<IActionResult> CreateProviderAccount(CreatePhysicianDTO model)
        {
            try
            {
                if (await helperService.IsAspNetUserEmailPresent(model.Email))
                {
                    ModelState.AddModelError(nameof(model.Email), "Email Already Present");
                    model.Regions = regionService.GetRegionList();
                    model.Roles = roleService.GetRoles();
                    return View(model);
                }
                await physicianService.CreatePhysician(model);
                TempData["Success"] = "Account created";
            }
            catch (Exception e)
            {
                TempData["Success"] = "Error in creating account";
            }

            return RedirectToAction("UserAccess", "Admin");
        }

        [CustomAuthorization("Provider,ProviderProfile")]
        [HttpGet("{physicianId}")]
        public IActionResult EditProviderAccount(int physicianId)
        {
            Physician? physician = physicianService.GetPhysicianById(physicianId);
            if (physician is not null)
            {
                EditPhysicianDTO model = physicianService.GetPhysicianInfo(physician);
                model.Roles = roleService.GetRoles();
                return View(model);
            }
            return View();
        }

        [CustomAuthorization("Provider,ProviderProfile")]
        public async Task<IActionResult> ResetPasswordProviderAsync(EditPhysicianDTO model)
        {
            Physician? physician;
            bool flag = false;
            if (helperService.IsPhysician())
            {
                int physicianId = physicianService.GetPhysicianIdByAspNetUserId(aspNetUserService.GetAspNetUserId());
                physician = physicianService.GetPhysicianById(physicianId);
                flag = true;
            }
            else
            {
                physician = physicianService.GetPhysicianById(model.PhysicianId);
            }

            if (physician is not null)
            {
                try
                {
                    await physicianService.ChangePassword(physician, model);
                    TempData["Success"] = "Data Updated";
                }
                catch (Exception e)
                {
                    TempData["Success"] = "Error in data updation";
                }
            }

            if (flag)
            {
                return RedirectToAction("MyProfileProvider", "Provider");
            }
            return RedirectToAction("EditProviderAccount", new { physicianId = model.PhysicianId });
        }

        [CustomAuthorization("Provider,ProviderProfile")]
        public async Task<IActionResult> UpdatePhysicianInfoAsync(EditPhysicianDTO model)
        {
            Physician? physician;
            bool flag = false;
            if (helperService.IsPhysician())
            {
                int physicianId = physicianService.GetPhysicianIdByAspNetUserId(aspNetUserService.GetAspNetUserId());
                physician = physicianService.GetPhysicianById(physicianId);
                flag = true;
            }
            else
            {
                physician = physicianService.GetPhysicianById(model.PhysicianId);
            }

            if (flag && physician.Email != model.Email && physician is not null)
            {
                await physicianService.UpdatePhysicianInfo(physician, model);
                return RedirectToAction("Logout");
            }

            if (physician is not null)
            {
                await physicianService.UpdatePhysicianInfo(physician, model);
            }

            if (flag)
            {
                TempData["Success"] = "Data Updated";
                return RedirectToAction("MyProfileProvider", "Provider");
            }
            TempData["Success"] = "Data Updated";
            return RedirectToAction("EditProviderAccount", new { physicianId = model.PhysicianId });
        }

        [CustomAuthorization("Provider,ProviderProfile")]
        public async Task<IActionResult> UpdatePhysicianBillingInfoAsync(EditPhysicianDTO model)
        {
            Physician? physician;
            bool flag = false;
            if (helperService.IsPhysician())
            {
                int physicianId = physicianService.GetPhysicianIdByAspNetUserId(aspNetUserService.GetAspNetUserId());
                physician = physicianService.GetPhysicianById(physicianId);
                flag = true;
            }
            else
            {
                physician = physicianService.GetPhysicianById(model.PhysicianId);
            }

            if (physician is not null)
            {
                try
                {
                    await physicianService.UpdateBillingInfo(physician, model);
                    TempData["Success"] = "Data updated";
                }
                catch (Exception e)
                {
                    TempData["Success"] = "Error in data updation";
                }
            }
            if (flag)
            {
                TempData["Success"] = "Data Updated";
                return RedirectToAction("MyProfileProvider", "Provider");
            }
            TempData["Success"] = "Data Updated";
            return RedirectToAction("EditProviderAccount", new { physicianId = model.PhysicianId });
        }

        [CustomAuthorization("Provider,ProviderProfile")]
        public async Task<IActionResult> UpdatePhysicianProfileInfo(EditPhysicianDTO model)
        {
            Physician? physician;
            bool flag = false;
            if (helperService.IsPhysician())
            {
                int physicianId = physicianService.GetPhysicianIdByAspNetUserId(aspNetUserService.GetAspNetUserId());
                physician = physicianService.GetPhysicianById(physicianId);
                flag = true;
            }
            else
            {
                physician = physicianService.GetPhysicianById(model.PhysicianId);
            }

            if (physician is not null)
            {
                try
                {
                    await physicianService.UpdateProfileInfo(physician, model);
                    TempData["Success"] = "Data Updated";
                }
                catch (Exception e)
                {
                    TempData["Error"] = "Error in data updation";
                }
            }
            if (flag)
            {
                return RedirectToAction("MyProfileProvider", "Provider");
            }
            return RedirectToAction("EditProviderAccount", new { physicianId = model.PhysicianId });
        }

        [CustomAuthorization("Vendorsinfo")]
        public async Task<IActionResult> Partners()
        {
            return View();
        }

        [CustomAuthorization("Vendorsinfo")]
        public async Task<IActionResult> VendorsTable(string searchVendor, int professionType, int page = 1, int itemsPerPage = 10)
        {
            Pagination<VendorsDTO>? filteredData = await healthProfessionalsService.GetFilteredHealthProfessionals(searchVendor, professionType, page, itemsPerPage);
            return PartialView("_VendorTable", filteredData);
        }

        [CustomAuthorization("Vendorsinfo")]
        [HttpGet("{vendorId}")]
        public async Task<IActionResult> EditBusiness(int vendorId)
        {
            EditBusinessDTO? model = await healthProfessionalsService.GetHealthProfessionalInfo(vendorId);
            model.Regions = regionService.GetRegionList();
            model.ProfessionList = healthProfessionalTypeService.GetProfession();

            return View(model);
        }

        [CustomAuthorization("Vendorsinfo")]
        [HttpPost("{vendorId}")]
        public async Task<IActionResult> EditBusiness(int vendorId, EditBusinessDTO model)
        {
            await healthProfessionalsService.EditProfessional(vendorId, model);
            return RedirectToAction("Partners", "Admin");
        }

        [CustomAuthorization("Vendorsinfo")]
        public async Task<IActionResult> AddBusiness()
        {
            EditBusinessDTO? model = new EditBusinessDTO();
            model.Regions = regionService.GetRegionList();
            model.ProfessionList = healthProfessionalTypeService.GetProfession();

            return View(model);
        }

        [CustomAuthorization("Vendorsinfo")]
        [HttpPost]
        public async Task<IActionResult> AddBusiness(EditBusinessDTO model)
        {
            await healthProfessionalsService.AddBusiness(model);
            return RedirectToAction("Partners", "Admin");
        }

        [CustomAuthorization("Vendorsinfo")]
        [HttpPost("{vendorId}")]
        public async Task<IActionResult> DeleteVendor(int vendorId)
        {
            await healthProfessionalsService.DeleteVendor(vendorId);
            return RedirectToAction("Partners", "Admin");
        }


        [CustomAuthorization("PatientRecords")]
        public async Task<IActionResult> PatientHistory()
        {
            return View();
        }

        [CustomAuthorization("PatientRecords")]
        public async Task<IActionResult> PatientHistoryTable(string firstName, string lastName, string email, string phoneNumber, int page = 1, int itemsPerPage = 10)
        {
            Pagination<PatientHistoryDTO>? filteredData = await userService.GetFilteredUsers(firstName, lastName, email, phoneNumber, page, itemsPerPage);
            return PartialView("_PatientHistoryTable", filteredData);
        }

        [CustomAuthorization("PatientRecords")]
        [HttpGet("{userId}")]
        public async Task<IActionResult> PatientRecords(int userId)
        {
            List<PatientRecordsDTO>? modelList = await requestServices.GetPatientRecord(userId);
            return View(modelList);
        }

        [CustomAuthorization("PatientRecords")]
        public async Task<IActionResult> SearchRecords()
        {
            return View();
        }

        [CustomAuthorization("PatientRecords")]
        public async Task<IActionResult> SearchRecordsTable(string patientName, string email, string phoneNumber, int requestStatus, int requestType, DateTime fromDateOfService, DateTime toDateOfService, string providerName, int page = 1, int itemsPerPage = 10)
        {
            Pagination<SearchRecordsDTO>? filteredData = await requestServices.GetfilteredSearchRecords(patientName, email, phoneNumber, requestStatus, requestType, fromDateOfService, toDateOfService, providerName, page, itemsPerPage);
            return PartialView("_SearchRecordsTable", filteredData);
        }

        [CustomAuthorization("PatientRecords")]
        public async Task<FileResult> ExportSearchRecords(string patientName, string email, string phoneNumber, int requestStatus, int requestType, DateTime fromDateOfService, DateTime toDateOfService, string providerName, int page = 1, int itemsPerPage = 10)
        {
            Pagination<SearchRecordsDTO>? records = await requestServices.GetfilteredSearchRecords(patientName, email, phoneNumber, requestStatus, requestType, fromDateOfService, toDateOfService, providerName, page, itemsPerPage);
            byte[]? file = ExcelHelper.CreateFile(records.Data);
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "patient_records.xlsx");
        }

        [CustomAuthorization("PatientRecords")]
        [HttpPost("{requestId}")]
        public async Task<IActionResult> DeletePatientRecord(int requestId)
        {
            await requestServices.DeletePatientRecord(requestId);
            return RedirectToAction("SearchRecords", "Admin");
        }

        [CustomAuthorization("Emaillogs")]
        public async Task<IActionResult> EmailLogs()
        {
            LogsDTO? model = new LogsDTO();
            model.Roles = roleService.GetRoles();
            return View(model);
        }

        [CustomAuthorization("Emaillogs")]
        public async Task<IActionResult> EmailLogsTable(int role, string receiverName, string emailId, DateTime createdDate, DateTime sentDate, int page = 1, int itemsPerPage = 10)
        {
            Pagination<LogsDTO>? filteredData = await emailLogService.GetFilteredEmailLogs(role, receiverName, emailId, createdDate, sentDate, page, itemsPerPage);
            return PartialView("_EmailLogsTable", filteredData);
        }

        [CustomAuthorization("SMSLogs")]
        public async Task<IActionResult> SmsLogs()
        {
            LogsDTO? model = new LogsDTO();
            model.Roles = roleService.GetRoles();
            return View(model);
        }

        [CustomAuthorization("SMSLogs")]
        public async Task<IActionResult> SmsLogsTable(int role, string receiverName, string phoneNumber, DateTime createdDate, DateTime sentDate, int page = 1, int itemsPerPage = 10)
        {
            Pagination<LogsDTO>? filteredData = await smsLogService.GetFilteredSmsLogs(role, receiverName, phoneNumber, createdDate, sentDate, page, itemsPerPage);
            return PartialView("_SmsLogsTable", filteredData);
        }

        [CustomAuthorization("Blocked History")]
        public async Task<IActionResult> BlockHistory()
        {
            return View();
        }

        [CustomAuthorization("Blocked History")]
        public async Task<IActionResult> BlockHistoryTable(string name, DateTime createdDate, string email, string phonenumber, int page = 1, int itemsPerPage = 10)
        {
            Pagination<BlockHistoryDTO>? filteredData = await blockRequestService.GetFilteredBlockedHistry(name, createdDate, email, phonenumber, page, itemsPerPage);
            return PartialView("_BlockHistoryTable", filteredData);
        }

        [CustomAuthorization("Blocked History")]
        [HttpPost("{requestId}")]
        public async Task<IActionResult> UnblockRequest(int requestId)
        {
            await blockRequestService.UnblockRequest(requestId);
            return RedirectToAction("BlockHistory", "Admin");
        }

        [CustomAuthorization("Dashboard")]
        public async Task<IActionResult> RequestSupport()
        {
            await handlePhysicianService.RequestDTYSupport();
            return RedirectToAction("AdminDashboard", "Admin");
        }

        [CustomAuthorization("Dashboard")]
        public async Task<IActionResult> Invoicing()
        {
            return View();
        }

        [CustomAuthorization("Dashboard")]
        public async Task<IActionResult> GetFinalizeTimesheetTable(string selectedvalue, int physicianid)
        {
            FinalizeTimesheetDTO model = await timesheetService.GetFinalizeTimesheetTable(selectedvalue, physicianid);
            if (model.isapproved != true && model.isfinalize == true)
            {
                return PartialView("_approvetablepartial", model);
            }
            else if (model.isfinalize == true && model.isapproved == true)
            {
                return PartialView("_timesheetpartial", model);
            }
            return PartialView("_timesheetpartial", model);
        }

        [CustomAuthorization("Dashboard")]
        public async Task<IActionResult> Approvetimesheet(string selectedValue, int physicianid)
        {
            FinalizeTimesheetDTO model = await timesheetService.Gettimesheet(selectedValue, physicianid);
            return View(model);
        }

        [CustomAuthorization("Dashboard")]
        public async Task<IActionResult> Approvetimesheetbutton(int id)
        {
            await timesheetService.Approvetimesheet(id);
            return RedirectToAction("Invoicing", "Admin");
        }

        [HttpPost]
        [CustomAuthorization("Dashboard")]
        public async Task<IActionResult> Posttimesheet(FinalizeTimesheetDTO model)
        {
            await timesheetService.Posttimesheet(model);
            return RedirectToAction("Invoicing", "Admin");
        }

        [HttpGet]
        [CustomAuthorization("Dashboard")]
        public IActionResult Payrate(int Physicianid)
        {
            Payrate p = payrateService.GetPayrate(Physicianid);
            PayrateDTO model = new PayrateDTO();
            if (p != null)
            {
                model = new PayrateDTO
                {
                    Shift = p.Shift,
                    NightShift_Weekend = p.NightshiftWeekend,
                    HouseCalls = p.Housecall,
                    HouseCalls_Nights_Weekend = p.HousecallnightWeekend,
                    PhoneConsults = p.PhoneConsult,
                    PhoneConsults_Nights_Weekend = p.PhoneConsultNightWeekend,
                    BatchTesting = p.BatchTesting
                };
            }
            model.Physicianid = Physicianid;
            return View(model);
        }

        [HttpPost]
        [CustomAuthorization("Dashboard")]
        public void Payrate(int Physicianid, int value, string paytype)
        {
            Payrate model = payrateService.SetPayrate(Physicianid, value, paytype);
        }

    }
}