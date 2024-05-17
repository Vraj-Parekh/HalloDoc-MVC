using Entities.DataContext;
using Entities.Models;
using HalloDoc.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
    public class HelperService : IHelperService
    {
        private readonly HalloDocDbContext _context;
        private readonly IAdminService adminService;
        private readonly IHttpContextAccessor httpContext;
        private readonly IEmailSender emailSender;
        private readonly IWebHostEnvironment env;
        private readonly IEmailLogService emailLogService;

        public HelperService(HalloDocDbContext _context, IAdminService adminService, IHttpContextAccessor httpContext, IEmailSender emailSender, IWebHostEnvironment env)
        {
            this._context = _context;
            this.adminService = adminService;
            this.httpContext = httpContext;
            this.emailSender = emailSender;
            this.env = env;
            this.emailLogService = emailLogService;
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

        public async Task SendAttachment(int request_id, int[] files_jx, string email)
        {
            string subject = "Your documents for request";
            string message = "See all the attachments";
            List<string> attach = new List<string>();
            foreach (var item in files_jx)
            {
                attach.Add(await GetFileAsync(item, request_id));
            }

            await emailSender.SendEmailAsync(email, subject, message, attach);
        }
        private string RootPathBuilder(string folder, string fileName) => Path.Combine(Path.Combine(env.WebRootPath, "uploads"), fileName);

        public async Task<string> GetFileAsync(int fileId, int requestId)
        {
            var file = await _context.Requestwisefiles.FirstOrDefaultAsync(a => a.Isdeleted == false && a.Requestwisefileid == fileId && a.Requestid == requestId);
            if (file is null)
                return null!;

            string filePath = RootPathBuilder("uploads", file.Filename);
            return filePath;
        }

        public string GetAspNetUserId() => httpContext.HttpContext?.User.FindFirst("userId")?.Value!;

        public Physician GetPhysician()
        {
            return _context.Physicians.Where(a => a.Aspnetuserid == GetAspNetUserId()).FirstOrDefault();
        }
        public async Task<bool> IsAspNetUserEmailPresent(string email)
        {
            return await _context.Aspnetusers.Where(a => a.Email == email).AnyAsync();
        }
     }
}
