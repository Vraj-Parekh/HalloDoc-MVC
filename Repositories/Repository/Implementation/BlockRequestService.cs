using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Types;

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

        public async Task<Pagination<BlockHistoryDTO>> GetFilteredBlockedHistry(string name,DateTime createdDate,string email,string phonenumber,int page,int itemsPerPage)
        {
            IQueryable<Blockrequest>? query = _context.Blockrequests.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                name = name.ToLower().Trim();
                query = query.Where(a => a.Email.ToLower().Contains(name));
            }

            if (!string.IsNullOrEmpty(email))
            {
                email = email.ToLower().Trim();
                query = query.Where(a => a.Email.ToLower().Contains(email));
            }

            if (!string.IsNullOrEmpty(phonenumber))
            {
                phonenumber = phonenumber.ToLower().Trim();
                query = query.Where(a => a.Phonenumber.ToLower().Contains(phonenumber));
            }

            if (createdDate != DateTime.MinValue)
            {
                query = query.Where(a => a.Createddate == createdDate);
            }

            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            int skip = (page - 1) * itemsPerPage;

            List<Blockrequest>? blockRequests = await query.Skip(skip)
                .Take(itemsPerPage)
                .ToListAsync();

            List<BlockHistoryDTO> modelList = new List<BlockHistoryDTO>();

            foreach (var item in blockRequests)
            {
                BlockHistoryDTO model = new BlockHistoryDTO()
                {
                    CreatedDate = item.Createddate?.ToString("MMM dd,yyy"),
                    Email = item.Email ?? "-",
                    IsActive = (bool)item.Isactive,
                    Notes = item.Reason??"-",
                    PatientName = "No idea",
                    PhoneNumber = item.Phonenumber??"-",
                    //RequestId = item.Requestid,
                };
                modelList.Add(model);
            }

            return new Pagination<BlockHistoryDTO>
            {
                Data = modelList,
                TotalPages = totalPages,
                CurrentPage = page
            };
        }
    }
}
