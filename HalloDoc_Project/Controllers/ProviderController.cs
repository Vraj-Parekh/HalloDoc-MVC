using Entities.Models;
using Entities.ViewModels;
using HalloDoc_Project.Attributes;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public ProviderController(IRegionService regionService, IRoleService roleService, IPhysicianService physicianService, IShiftService shiftService, IShiftDetailService shiftDetailService, IShiftDetailRegionService shiftDetailRegionService,IHealthProfessionalsService healthProfessionalsService,IHealthProfessionalTypeService healthProfessionalTypeService,IUserService userService,IRequestServices requestServices,IBlockRequestService blockRequestService,IEmailLogService emailLogService,ISmsLogService smsLogService)
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
            Pagination<LogsDTO>? filteredData = await emailLogService.GetFilteredEmailLogs(role, receiverName, phoneNumber, createdDate, sentDate, page, itemsPerPage);
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
    }
}
