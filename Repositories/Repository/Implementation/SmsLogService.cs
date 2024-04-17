using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
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
        private readonly IRequestServices requestServices;

        public SmsLogService(HalloDocDbContext _context, IHelperService helperService, IAdminService adminService, IPhysicianService physicianService,IRequestServices requestServices)
        {
            this._context = _context;
            this.helperService = helperService;
            this.adminService = adminService;
            this.physicianService = physicianService;
            this.requestServices = requestServices;
        }

        public async Task AddSmsLog(string phoneNumber, string message, bool isSmsSent,Request request = null!)
        {
            Smslog smsLog = new Smslog()
            {
                Createdate = DateTime.Now,
                Sentdate = DateTime.Now,
                Mobilenumber = phoneNumber,
                Smstemplate = message,
                Senttries = 1,
                Issmssent = isSmsSent,
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

        public async Task<Pagination<LogsDTO>> GetFilteredSmsLogs(int role, string receiverName, string phoneNumber, DateTime createdDate, DateTime sentDate, int page, int itemsPerPage)
        {
            IQueryable<Smslog>? query = _context.Smslogs.AsQueryable();

            if (!string.IsNullOrEmpty(receiverName))
            {
                receiverName = receiverName.ToLower().Trim();
                //pending work
            }
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                phoneNumber = phoneNumber.ToLower().Trim();
                query = query.Where(a => a.Mobilenumber.ToLower().Contains(phoneNumber));
            }
            if (createdDate != DateTime.MinValue)
            {
                query = query.Where(a => a.Createdate.Date == createdDate.Date);
            }
            if (sentDate != DateTime.MinValue)
            {
                query = query.Where(a => a.Sentdate.Value.Date == sentDate.Date);
            }

            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            if (page < 1) page = 1;

            int skip = (page - 1) * itemsPerPage;

            List<Smslog>? smsLogs = await query.Skip(skip)
                .Take(itemsPerPage)
                .ToListAsync();

            List<LogsDTO> modelList = new List<LogsDTO>();

            foreach (var item in smsLogs)
            {
                LogsDTO model = new LogsDTO()
                {
                    Active = 1,
                    RoleName = item.Roleid.ToString(),
                    PhoneNumber = item.Mobilenumber,
                    CreatedDate = item.Createdate.ToString("MMM dd,yyy"),
                    SentDate = item.Sentdate?.ToString("MMM dd,yyy"),
                    Sent = (item.Issmssent == true) ? "Yes" : "No",
                    SentTries = (int)item.Senttries,
                    ConfirmationNumber = item.Confirmationnumber ?? "-",
                };
                if (item.Requestid is not null)
                {
                    Request? request = requestServices.GetRequest((int)item.Requestid);
                    model.Recipient = request.Firstname ?? "";
                }
                modelList.Add(model);
            }

            return new Pagination<LogsDTO>
            {
                Data = modelList,
                TotalPages = totalPages,
                CurrentPage = page
            };
        }
    }
}
