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
    public class ShiftDetailService: IShiftDetailService
    {
        private readonly HalloDocDbContext _context;
        private readonly IAspNetUserService aspNetUserService;

        public ShiftDetailService(HalloDocDbContext _context, IAspNetUserService aspNetUserService)
        {
            this._context = _context;
            this.aspNetUserService = aspNetUserService;
        }

        public async Task<List<Shiftdetail>> GetShiftDetails()
        {
            return await _context.Shiftdetails.Include(a=>a.Shift).ToListAsync();
        }

        public async Task<Shiftdetail> AddShiftDetails(Shift shift, CreateShiftDTO model)
        {
            Shiftdetail shiftdetail = new Shiftdetail()
            {
                Shiftid = shift.Shiftid,
                Regionid = model.RegionId,
                Starttime = model.StartTime,
                Endtime = model.EndTime,
                Status = 1,//pending for review
                Isdeleted = false,
                Modifiedby = aspNetUserService.GetAspNetUserId(),
            };
            _context.Add(shiftdetail);
            await _context.SaveChangesAsync();
            return shiftdetail;
        }
    }
}
