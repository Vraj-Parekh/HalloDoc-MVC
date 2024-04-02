using Entities.Models;
using Entities.ViewModels;
using HalloDoc_Project.Attributes;
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

        public ProviderController(IRegionService regionService,IRoleService roleService)
        {
            this.regionService = regionService;
            this.roleService = roleService;
        }

        public IActionResult CreateProviderAccount()
        {
            CreatePhysicianDTO? model = new CreatePhysicianDTO();
            model.Regions = regionService.GetRegion();
            model.Roles = roleService.GetRoles();
            
            return View(model);
        }

        public IActionResult EditProviderAccount()
        {
            return View();
        }
    }
}
