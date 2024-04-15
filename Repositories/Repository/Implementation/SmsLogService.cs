using Entities.DataContext;
using Entities.Models;
using NuGet.Packaging;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Implementation
{
    public class SmsLogService: ISmsLogService
    {
        private readonly HalloDocDbContext _context;
        private readonly IHelperService helperService;
        private readonly IAdminService adminService;
        private readonly IPhysicianService physicianService;

        public SmsLogService(HalloDocDbContext _context, IHelperService helperService, IAdminService adminService, IPhysicianService physicianService)
        {
            this._context = _context;
            this.helperService = helperService;
            this.adminService = adminService;
            this.physicianService = physicianService;
        }

        public async void AddSmsLog(string phoneNumber, string message, Request request = null!)
        {
            Smslog smsLog = new Smslog()
            {
                Createdate = DateTime.Now,
                Sentdate = DateTime.Now,
                Mobilenumber = phoneNumber,
                Smstemplate = message,
                Senttries = 1,
            };

            if (helperService.IsAdmin())
            {
                string? adminEmail = helperService.GetLoggedinEmail();
                Admin? admin = adminService.GetAdmin(adminEmail);
                if (admin is not null)
                {
                    smsLog.Roleid = admin.Roleid;
                    smsLog.Adminid = admin.Adminid;
                }
            }
            else if (helperService.IsPhysician())
            {
                string? physicianEmail = helperService.GetLoggedinEmail();
                Physician? physician = physicianService.GetPhysician(physicianEmail);
                if (physician is not null)
                {
                    smsLog.Roleid = physician.Roleid;
                    smsLog.Physicianid = physician.Physicianid;
                }
            }

            if (request is not null)
            {
                smsLog.Confirmationnumber = request.Confirmationnumber;
                smsLog.Requestid = request.Requestid;
            }

            _context.Add(smsLog);
            await _context.SaveChangesAsync();
        }
    }
}
