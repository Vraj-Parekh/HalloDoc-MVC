using Entities.DataContext;
using Entities.Models;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Implementation
{
    public class EmailLogService: IEmailLogService
    {
        private readonly HalloDocDbContext _context;

        public EmailLogService(HalloDocDbContext _context)
        {
            this._context = _context;
        }

        public void AddEmailLogs(string email, string message, string subject, List<string>? attachments = null, Request request = null!)
        {

        }
    }
}
