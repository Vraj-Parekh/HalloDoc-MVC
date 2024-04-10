using Entities.Models;
using Entities.ViewModels;
using HalloDoc_Project.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Repository.Interface;

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

        public ProviderController(IRegionService regionService, IRoleService roleService, IPhysicianService physicianService, IShiftService shiftService, IShiftDetailService shiftDetailService, IShiftDetailRegionService shiftDetailRegionService,IHealthProfessionalsService healthProfessionalsService,IHealthProfessionalTypeService healthProfessionalTypeService)
        {
            this.regionService = regionService;
            this.roleService = roleService;
            this.physicianService = physicianService;
            this.shiftService = shiftService;
            this.shiftDetailService = shiftDetailService;
            this.shiftDetailRegionService = shiftDetailRegionService;
            this.healthProfessionalsService = healthProfessionalsService;
            this.healthProfessionalTypeService = healthProfessionalTypeService;
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

        public async Task<IActionResult> VendorsTable(string searchVendor,int professionType)
        {
            List<VendorsDTO>? filteredData = await healthProfessionalsService.GetHealthProfessionals(searchVendor, professionType);
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
    }
}
