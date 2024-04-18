using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Org.BouncyCastle.Utilities;
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
        private readonly IPhysicianService physicianService;
        private readonly IHelperService helperService;

        public ShiftDetailService(HalloDocDbContext _context, IAspNetUserService aspNetUserService, IPhysicianService physicianService, IHelperService helperService)
        {
            this._context = _context;
            this.aspNetUserService = aspNetUserService;
            this.physicianService = physicianService;
            this.helperService = helperService;
        }

        public async Task<List<Shiftdetail>> GetShiftDetails()
        {
            var physicians = await _context.Shiftdetails.Include(a => a.Shift).ThenInclude(a => a.Physician).Where(a => a.Isdeleted == false).ToListAsync();
            if (helperService.IsPhysician())
            {
                int physicianId = physicianService.GetPhysicianIdByAspNetUserId(aspNetUserService.GetAspNetUserId());
                physicians = physicians.Where(a => a.Shift.Physicianid == physicianId).ToList();
            }
            return physicians;
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

        public async Task ApproveShift(List<int> shiftDetailIds)
        {
            foreach (int shiftid in shiftDetailIds)
            {
                Shiftdetail? shift = await _context.Shiftdetails.Where(a => a.Shiftdetailid == shiftid).FirstOrDefaultAsync();

                shift.Status = 2;

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

        public async Task DeleteSelectedShift(List<int> shiftDetailIds)
        {
            foreach (int shiftid in shiftDetailIds)
            {
                Shiftdetail? shift = await _context.Shiftdetails.Where(a => a.Shiftdetailid == shiftid).FirstOrDefaultAsync();

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

        public async Task<List<Shiftdetail>> GetShiftsOnDate(DateTime date, int physicianId = 0)
        {
            var shifts = await _context.Shiftdetails.Where(a => a.Shiftdate == date.Date).Include(a => a.Shift).ToListAsync();
            shifts = shifts.Where(a => a.Shift.Physicianid == physicianId || physicianId == 0).ToList();
            shifts = shifts.Where(a => a.Isdeleted == false).ToList();

            return shifts;
        }
        public async Task<MdOncallDTO> GetOnCallData(int regionId)
        {
            bool onCall = true;
            List<Physician>? physicians = physicianService.GetPhysicianByRegionId(regionId);

            List<Physicianforcall> OnDutyModelList = new List<Physicianforcall>();
            List<Physicianforcall> OffDutyModelList = new List<Physicianforcall>();

            foreach (var physician in physicians)
            {
                List<Shiftdetail>? shiftDetails = await GetShiftsOnDate(DateTime.Now, physician.Physicianid);

                if (shiftDetails.Count > 0)
                {
                    TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);
                    foreach (var shift in shiftDetails)
                    {
                        if (shift.Shift.Physician == physician && (shift.Starttime > currentTime || shift.Endtime < currentTime))
                        {
                            onCall = false;
                        }
                        else
                        {
                            onCall = true;
                        }
                    }

                    if (onCall)
                    {
                        Physicianforcall onduty = new Physicianforcall()
                        {
                            PhysicianName = physician.Firstname,
                            Photo = (physician.Photo != null) ? "/uploads/" + physician.Photo : "/uploads/Person.jpg",
                        };
                        OnDutyModelList.Add(onduty);

                    }
                    else
                    {
                        Physicianforcall offduty = new Physicianforcall()
                        {
                            PhysicianName = physician.Firstname,
                            Photo = (physician.Photo != null) ? "/uploads/" + physician.Photo : "/uploads/Person.jpg",
                        };
                        OffDutyModelList.Add(offduty);
                    }
                }
                else
                {
                    Physicianforcall offduty = new Physicianforcall()
                    {
                        PhysicianName = physician.Firstname,
                        Photo = (physician.Photo != null) ? "/uploads/" + physician.Photo : "/uploads/Person.jpg",
                    };
                    OffDutyModelList.Add(offduty);
                    onCall = false;
                }
            }
            return new MdOncallDTO
            {
                onCall = OnDutyModelList,
                offDuty = OffDutyModelList,
            };
        }
    }
}
