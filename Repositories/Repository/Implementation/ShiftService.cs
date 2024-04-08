using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Implementation
{
    public class ShiftService: IShiftService
    {
        private readonly HalloDocDbContext _context;
        private readonly IAspNetUserService aspNetUserService;

        public ShiftService(HalloDocDbContext _context,IAspNetUserService aspNetUserService)
        {
            this._context = _context;
            this.aspNetUserService = aspNetUserService;
        }

        public async Task<Shift> AddShift(Physician physician,CreateShiftDTO model)
        {
            Shift shift = new Shift()
            {
                Physicianid = model.PhysicianId,
                Startdate = DateOnly.FromDateTime(model.ShiftDate),
                Isrepeat = model.Repeat,
                Repeatupto = model.RepeatUpto,
                Createdby = aspNetUserService.GetAspNetUserId(),
                Createddate = DateTime.Now,
            };
            _context.Add(shift);
            await _context.SaveChangesAsync();

            return shift;
        }
    }
}
