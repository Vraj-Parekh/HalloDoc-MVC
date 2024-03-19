using Entities.DataContext;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Repositories.Repository.Implementation
{
    public class RequestStatusLogServices : IRequestStatusLogServices
    {
        private readonly HalloDocDbContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;

        public RequestStatusLogServices(HalloDocDbContext _context, IHttpContextAccessor httpContextAccessor)
        {
            this._context = _context;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<Requeststatuslog> AddRequestStatusLogAsync(Request request, RequestStatus status)
        {
            Requeststatuslog requeststatuslog = new Requeststatuslog()
            {
                Requestid = request.Requestid,
                Status = (short)status,
                Createddate = DateTime.Now,
                Physician = request.Physician,
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

        public List<Requeststatuslog>? GetTransferNotes(int requestId)
        {
            return _context.Requeststatuslogs.Where(a=>a.Requestid == requestId).ToList();
        }

        public void AddCancelNote(int requestId, string reason, string notes)
        {
            Request? request = _context.Requests.FirstOrDefault(a => a.Requestid == requestId);

            if (request != null)
            {
                request.Status = 3;
                request.Modifieddate = DateTime.Now;
                request.Casetag = reason;
                _context.Requests.Update(request);

                Requeststatuslog requeststatuslog = new Requeststatuslog()
                {
                    Requestid = requestId,
                    Status = request.Status,
                    Notes = notes,
                    Createddate = DateTime.Now,
                };

                _context.Requeststatuslogs.Add(requeststatuslog);
                _context.SaveChanges();
            }

        }


    }
}

