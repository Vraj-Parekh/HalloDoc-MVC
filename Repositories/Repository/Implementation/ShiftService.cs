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
using Twilio.TwiML.Voice;

namespace Repositories.Repository.Implementation
{
    public class ShiftService : IShiftService
    {
        private readonly HalloDocDbContext _context;
        private readonly IAspNetUserService aspNetUserService;
        private readonly IHelperService helperService;

        public ShiftService(HalloDocDbContext _context, IAspNetUserService aspNetUserService, IHelperService helperService)
        {
            this._context = _context;
            this.aspNetUserService = aspNetUserService;
            this.helperService = helperService;
        }

        public async Task<Shift> AddShift(Physician physician, CreateShiftDTO model)
        {
            Shift shift = new Shift();

            shift.Physicianid = physician.Physicianid;
            shift.Startdate = DateOnly.FromDateTime(model.ShiftDate);
            shift.Isrepeat = model.Repeat;
            shift.Repeatupto = model.RepeatUpto;
            shift.Createdby = aspNetUserService.GetAspNetUserId();
            shift.Createddate = DateTime.Now;

            _context.Add(shift);
            await _context.SaveChangesAsync();

            return shift;
        }
        public async Task<Pagination<RequestedShiftDTO>> GetFilteredRequestedShifts(int regionId, bool isDateFilter, int page, int itemsPerPage)
        {
            IQueryable<Shift>? query = _context.Shifts
                    .Include(a => a.Shiftdetails)
                    .Where(a=>a.Shiftdetails.Any(a=>!a.Isdeleted && a.Status == 1))//1 means pending
                    .Include(a => a.Physician)
                    .AsQueryable();


            if (regionId > 0)
            {
                query = query.Where(a => a.Physician.Regionid == regionId);
            }
            if (isDateFilter)
            {
                query = query.Where(a => a.Shiftdetails.Any(sd => sd.Shiftdate.Month == DateTime.Now.Month));
            }

            int totalItems = await query.CountAsync();
           

            if (page < 1) page = 1;

            int skip = (page - 1) * itemsPerPage;

            List<Shift>? shifts = await query
                .Skip(skip)
                .Take(itemsPerPage)
                .ToListAsync();

            List<RequestedShiftDTO> modelList = new List<RequestedShiftDTO>();

            foreach (var item in shifts)
            {
                Shiftdetail? shiftDetail = item.Shiftdetails.FirstOrDefault();
                if (shiftDetail != null)
                {

                    RequestedShiftDTO model = new RequestedShiftDTO()
                    {
                        Staff = item.Physician.Firstname,
                        Day = shiftDetail?.Shiftdate.ToString("MMM dd,yyy"),
                        Time = shiftDetail?.Starttime.ToString("h:mm tt") + " - " + shiftDetail?.Endtime.ToString("h:mm tt"),
                        RegionName = helperService.GetRegionById((int)shiftDetail.Regionid),
                        ShiftDetailId = shiftDetail.Shiftdetailid,
                    };
                    modelList.Add(model);
                }
                else
                {
                    totalItems--;
                }
            }
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);
            return new Pagination<RequestedShiftDTO>
            {
                Data = modelList,
                TotalPages = totalPages,
                CurrentPage = page
            };
        }
    }
}
