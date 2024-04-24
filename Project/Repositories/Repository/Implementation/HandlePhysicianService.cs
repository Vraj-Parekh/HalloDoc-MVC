using Entities.DataContext;
using Entities.Models;
using HalloDoc.Utility;
using Microsoft.EntityFrameworkCore;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Implementation
{
    public class HandlePhysicianService : IHandlePhysicianService
    {
        private readonly HalloDocDbContext _context;
        private readonly IPhysicianService physicianService;
        private readonly IEmailLogService emailLogService;
        private readonly IEmailSender emailSender;

        public HandlePhysicianService(HalloDocDbContext _context, IPhysicianService physicianService, IEmailLogService emailLogService, IEmailSender emailSender)
        {
            this._context = _context;
            this.physicianService = physicianService;
            this.emailLogService = emailLogService;
            this.emailSender = emailSender;
        }

        public async Task RequestDTYSupport()
        {
            List<Physician>? physiciansWithoutShift = await _context.Physicians
                .Where(a => !_context.Shifts.Any(shift => shift.Physicianid == a.Physicianid))
                .ToListAsync();

            foreach (var physician in physiciansWithoutShift)
            {
                string email = physician.Email;
                string subject = "Reminder: Shift Assignment";
                string message = "Dear Dr. " + physician.Lastname + ",\n\n" +
                                 "This is a reminder that you have not been assigned any shifts yet. " +
                                 "Please contact the scheduling department to discuss your availability.\n\n" +
                                 "Best regards,\n" +
                                 "Your Hospital Team";
                try
                {
                    // Send email
                    await emailSender.SendEmailAsync(email, subject, message);
                    await emailLogService.AddEmailLog(email, message, subject, true);
                }
                catch (Exception ex)
                {
                    await emailLogService.AddEmailLog(email, message, subject, false);
                }
            }

        }
    }
}
