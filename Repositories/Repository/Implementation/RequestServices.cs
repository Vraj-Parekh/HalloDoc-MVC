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
    public class RequestServices : IRequestServices
    {
        private readonly HalloDocDbContext _context;
        private readonly IRequestStatusLogServices requestStatusLogServices;

        public RequestServices(HalloDocDbContext _context,IRequestStatusLogServices requestStatusLogServices)
        {
            this._context = _context;
            this.requestStatusLogServices = requestStatusLogServices;
        }
        public bool IsRequestPending(int requestId, string email)
        {
            return _context.Requests.FirstOrDefault(a => a.Requestid == requestId)?.Status == (int)RequestStatus.Pending;
        }

        public async Task<bool> AgreeWithAgreementAsync(int requestId)
        {
            Request? request = _context.Requests.FirstOrDefault(a => a.Requestid == requestId);
            if (request is null)
                return false;

            request.Status = (int)RequestStatus.Active;
            _context.Requests.Update(request);

            await requestStatusLogServices.AddRequestStatusLogAsync(request, RequestStatus.Active);


            int changes = await _context.SaveChangesAsync();
            if (changes > 0)
            {
                return true;
            }
            return false;
        }

        //Reject the Agreement
        public async Task<bool> RejectAgreementAsync(int requestId, string message)
        {

            Request? request = _context.Requests.FirstOrDefault(a => a.Requestid == requestId);
            if (request is null)
                return false;

            request.Status = (int)RequestStatus.ToClosed;
            _context.Requests.Update(request);

            await requestStatusLogServices.AddRequestStatusLogAsync(request, RequestStatus.ToClosed, message);
            int changes = await _context.SaveChangesAsync();
            if (changes > 0)
            {
                return true;
            }
            return false;
        }

    }
}
