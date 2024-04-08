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

        public ProviderController(IRegionService regionService, IRoleService roleService, IPhysicianService physicianService, IShiftService shiftService, IShiftDetailService shiftDetailService, IShiftDetailRegionService shiftDetailRegionService)
        {
            this.regionService = regionService;
            this.roleService = roleService;
            this.physicianService = physicianService;
            this.shiftService = shiftService;
            this.shiftDetailService = shiftDetailService;
            this.shiftDetailRegionService = shiftDetailRegionService;
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
            //files photo pending
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

        public async Task<IActionResult> CreateShift(CreateShiftDTO model)
        {
            Physician? physician = physicianService.GetPhysicianById(model.PhysicianId);
            if (physician is not null)
            {
                Shift? shift = await shiftService.AddShift(physician, model);
                Shiftdetail? shiftDetail = await shiftDetailService.AddShiftDetails(shift, model);
                await shiftDetailRegionService.AddShiftDetailRegion(shiftDetail, model);
                return RedirectToAction("Scheduling", "Admin");
            }
            return BadRequest("Physician not found");
        }
    }
}
