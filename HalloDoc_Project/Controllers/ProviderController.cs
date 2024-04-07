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

        public ProviderController(IRegionService regionService,IRoleService roleService,IPhysicianService physicianService)
        {
            this.regionService = regionService;
            this.roleService = roleService;
            this.physicianService = physicianService;
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
            return RedirectToAction("UserAccess","Admin");
        }

        [HttpGet("{physicianId}")]
        public IActionResult EditProviderAccount(int physicianId)
        {
            Physician? physician = physicianService.GetPhysicianById(physicianId);
            if(physician is not null)
            {
                //add physician region 
            }
            return View();
        }
    }
}
