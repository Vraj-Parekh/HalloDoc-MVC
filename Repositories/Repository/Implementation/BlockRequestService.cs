using Entities.DataContext;
using Entities.Models;
using Org.BouncyCastle.Utilities;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Implementation
{
    public class BlockRequestService : IBlockRequestService
    {
        private readonly HalloDocDbContext _context;
        private readonly IRequestServices requestServices;
        private readonly IRequestStatusLogServices requestStatusLogServices;

        public BlockRequestService(HalloDocDbContext _context, IRequestServices requestServices, IRequestStatusLogServices requestStatusLogServices)
        {
            this._context = _context;
            this.requestServices = requestServices;
            this.requestStatusLogServices = requestStatusLogServices;
        }

        public void BlockRequest(int requestId, string reason)
        {
            Request? request = requestServices.GetRequest(requestId);

            if (request != null)
            {
                request.Status = 20;
                request.Modifieddate = DateTime.Now;

                Requeststatuslog requeststatuslog = new Requeststatuslog()
                {
                    Requestid = requestId,
                    Status = request.Status,
                    Createddate = DateTime.Now,
                };

                Blockrequest blockrequest = new Blockrequest()
                {
                    Requestid = request.Requestid.ToString(),
                    Modifieddate = DateTime.Now,
                    Reason = reason,
                };

                _context.Requests.Update(request);
                _context.Requeststatuslogs.Add(requeststatuslog);
                _context.Blockrequests.Add(blockrequest);

                _context.SaveChanges();
            }
        }
    }
}
