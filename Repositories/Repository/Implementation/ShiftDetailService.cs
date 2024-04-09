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
    public class ShiftDetailService : IShiftDetailService
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
            return await _context.Shiftdetails.Include(a => a.Shift).ThenInclude(a => a.Physician).Where(a => a.Isdeleted == false).ToListAsync();
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
                Shiftdate = model.ShiftDate,
        };
            _context.Add(shiftdetail);
            await _context.SaveChangesAsync();
            return shiftdetail;
        }

        public async Task ChangeShiftStatus(int shiftDetailId)
        {
            Shiftdetail? shift = await _context.Shiftdetails.Where(a => a.Shiftdetailid == shiftDetailId).FirstOrDefaultAsync();

            if (shift != null)
            {
                if (shift.Status == 1)
                {
                    shift.Status = 2;
                }
                else
                {
                    shift.Status = 1;
                }

                _context.Update(shift);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteShift(int shiftDetailId)
        {
            Shiftdetail? shift = await _context.Shiftdetails.Where(a => a.Shiftdetailid == shiftDetailId).FirstOrDefaultAsync();
            if (shift != null)
            {
                shift.Isdeleted = true;

                _context.Update(shift);
                await _context.SaveChangesAsync();
            }
        }

        public async Task EditShift(ScheduleDTO model)
        {
            Shiftdetail? shift = await _context.Shiftdetails
                .Where(a => a.Shiftdetailid == model.ShiftDetailId)
                .Include(s => s.Shift)
                .FirstOrDefaultAsync();
            if (shift != null)
            {
                shift.Shiftdate = new DateTime(model.Startdate.Year, model.Startdate.Month, model.Startdate.Day);
                shift.Starttime = model.Starttime;
                shift.Endtime = model.Endtime;
                _context.Update(shift);

                if (shift.Shift != null)
                {
                    shift.Shift.Startdate = model.Startdate;
                    _context.Update(shift.Shift);
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}
