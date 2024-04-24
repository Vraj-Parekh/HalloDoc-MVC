using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Implementation
{
    public class EmailLogService : IEmailLogService
    {
        private readonly HalloDocDbContext _context;
        private readonly IHelperService helperService;
        private readonly IAdminService adminService;
        private readonly IPhysicianService physicianService;
        private readonly IRequestServices requestServices;

        public EmailLogService(HalloDocDbContext _context, IHelperService helperService, IAdminService adminService, IPhysicianService physicianService, IRequestServices requestServices)
        {
            this._context = _context;
            this.helperService = helperService;
            this.adminService = adminService;
            this.physicianService = physicianService;
            this.requestServices = requestServices;
        }

        public async Task AddEmailLog(string email, string message, string subject, bool isEmailSent, List<string>? attachments = null, Request request = null!)
        {
            Emaillog emailLog = new Emaillog()
            {
                Createdate = DateTime.Now,
                Sentdate = DateTime.Now,
                Emailid = email,
                Emailtemplate = message,
                Subjectname = subject,
                Senttries = 1,
                Isemailsent = isEmailSent,
            };

            if (helperService.IsAdmin())
            {
                string? adminEmail = helperService.GetLoggedinEmail();
                Admin? admin = adminService.GetAdmin(adminEmail);
                if (admin is not null)
                {
                    emailLog.Roleid = admin.Roleid;
                    emailLog.Adminid = admin.Adminid;
                }
            }
            else if (helperService.IsPhysician())
            {
                string? physicianEmail = helperService.GetLoggedinEmail();
                Physician? physician = physicianService.GetPhysician(physicianEmail);
                if (physician is not null)
                {
                    emailLog.Roleid = physician.Roleid;
                    emailLog.Physicianid = physician.Physicianid;
                }
            }

            if (request is not null)
            {
                emailLog.Confirmationnumber = request.Confirmationnumber;
                emailLog.Requestid = request.Requestid;
            }

            _context.Add(emailLog);
            await _context.SaveChangesAsync();
        }

        public async Task<Pagination<LogsDTO>> GetFilteredEmailLogs(int role, string receiverName, string emailId, DateTime createdDate, DateTime sentDate, int page, int itemsPerPage)
        {
            IQueryable<Emaillog>? query = _context.Emaillogs.AsQueryable();

            if (!string.IsNullOrEmpty(receiverName))
            {
                receiverName = receiverName.ToLower().Trim();
                //pending work
            }
            if (!string.IsNullOrEmpty(emailId))
            {
                emailId = emailId.ToLower().Trim();
                query = query.Where(a => a.Emailid.ToLower().Contains(emailId));
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

            List<Emaillog>? emailLogs = await query.Skip(skip)
                .Take(itemsPerPage)
                .ToListAsync();

            List<LogsDTO> modelList = new List<LogsDTO>();

            foreach (var item in emailLogs)
            {
                LogsDTO model = new LogsDTO()
                {
                    Active = 1,
                    RoleName = item.Roleid.ToString(),
                    Email = item.Emailid,
                    CreatedDate = item.Createdate.ToString("MMM dd,yyy"),
                    SentDate = item.Sentdate?.ToString("MMM dd,yyy"),
                    Sent = (item.Isemailsent == true) ? "Yes" : "No",
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
