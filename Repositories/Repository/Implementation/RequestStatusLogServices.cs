using Entities.DataContext;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Implementation
{
    public class RequestStatusLogServices : IRequestStatusLogServices
    {
        private readonly HalloDocDbContext _context;

        public RequestStatusLogServices(HalloDocDbContext _context)
        {
            this._context = _context;
        }
        public async Task<Requeststatuslog> AddRequestStatusLogAsync(Request request, RequestStatus status)
        {
            Requeststatuslog requeststatuslog = new Requeststatuslog()
            {
                Requestid = request.Requestid,
                Status = (short)status,
                Createddate = DateTime.Now,
                Physician = request.Physician
            };

            _context.Requeststatuslogs.Add(requeststatuslog);
            await _context.SaveChangesAsync();

            return requeststatuslog;
        }

        public async Task<Requeststatuslog> AddRequestStatusLogAsync(Request request, RequestStatus status, string message)
        {
            Requeststatuslog requeststatuslog = new Requeststatuslog()
            {
                Requestid = request.Requestid,
                Status = (short)status,
                Notes = message,
                Createddate = DateTime.Now,
                Physician = request.Physician
            };

            _context.Requeststatuslogs.Add(requeststatuslog);
            await _context.SaveChangesAsync();

            return requeststatuslog;
        }

        public Requeststatuslog? GetTransferNotes(int requestId)
        {
            return _context.Requeststatuslogs.FirstOrDefault(a=>a.Requestid == requestId);
        }
    }
}

