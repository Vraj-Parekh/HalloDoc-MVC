using Entities.DataContext;
using Microsoft.AspNetCore.Http;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Implementation
{
    public class HelperService: IHelperService
    {
        private readonly HalloDocDbContext _context;
        private readonly IAdminService adminService;
        private readonly IPhysicianService physicianService;
        private readonly IHttpContextAccessor httpContext;

        public HelperService(HalloDocDbContext _context,IAdminService adminService,IPhysicianService physicianService, IHttpContextAccessor httpContext)
        {
            this._context = _context;
            this.adminService = adminService;
            this.physicianService = physicianService;
            this.httpContext = httpContext;
        }

        public List<string> GetRoles()
        {
            return httpContext.HttpContext?.User.FindAll(a => a.Type == ClaimTypes.Role).Select(a => a.Value).ToList()!;
        }
        public bool IsAdmin()
        {
            return GetRoles().Contains("Admin");
        }

        public bool IsPhysician()
        {
            return GetRoles().Contains("Provider");
        }
        public string GetLoggedinEmail()
        {
            return httpContext.HttpContext?.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.Email)?.Value;
        }
        public string GetRegionById(int regionId)
        {
            return _context.Regions.Where(a => a.Regionid == regionId).Select(a => a.Name).FirstOrDefault();
        }
    }
}
