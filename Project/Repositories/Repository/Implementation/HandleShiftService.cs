using Entities.DataContext;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Repositories.Repository.Interface;

namespace Repositories.Repository.Implementation
{
    public class HandleShiftService : IHandleShiftService
    {
        private readonly HalloDocDbContext _context;
        private readonly IShiftService shiftService;
        private readonly IShiftDetailService shiftDetailService;
        private readonly IShiftDetailRegionService shiftDetailRegionService;
        private readonly IHelperService helperService;
        private readonly IPhysicianService physicianService;
        private readonly IAspNetUserService aspNetUserService;

        public HandleShiftService(HalloDocDbContext _context, IShiftService shiftService, IShiftDetailService shiftDetailService, IShiftDetailRegionService shiftDetailRegionService, IHelperService helperService, IPhysicianService physicianService, IAspNetUserService aspNetUserService)
        {
            this._context = _context;
            this.shiftService = shiftService;
            this.shiftDetailService = shiftDetailService;
            this.shiftDetailRegionService = shiftDetailRegionService;
            this.helperService = helperService;
            this.physicianService = physicianService;
            this.aspNetUserService = aspNetUserService;
        }

        public async Task AddShiftDetails(CreateShiftDTO model)
        {
            Physician physician;
            if (helperService.IsPhysician())
            {
                int physicianId = physicianService.GetPhysicianIdByAspNetUserId(aspNetUserService.GetAspNetUserId());
                physician = physicianService.GetPhysicianById(physicianId);
            }
            else
            {
                physician = physicianService.GetPhysicianById(model.PhysicianId);
            }

            using (IDbContextTransaction? transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    bool shiftCreated = false;
                    Shift? shift = await shiftService.AddShift(physician, model);
                    Shiftdetail? shiftDetail = await shiftDetailService.AddShiftDetails(shift, model);
                    if (shiftDetail is not null)
                    {
                        shiftCreated = true;
                        await shiftDetailRegionService.AddShiftDetailRegion(shiftDetail, model);
                    }
                    if (model.Repeat && model.Repeat_Days is not null)
                    {
                        for (int i = 0; i < model.RepeatUpto; i++)
                        {
                            foreach (var item in model.Repeat_Days)
                            {
                                int start = (int)model.ShiftDate.DayOfWeek;                                int target = item;                                if (target <= start)                                    target += 7;                                model.ShiftDate = model.ShiftDate.AddDays(target - start);                                shiftDetail = await shiftDetailService.AddShiftDetails(shift, model);
                                if(shiftDetail is not null)
                                {
                                    shiftCreated = true;
                                    await shiftDetailRegionService.AddShiftDetailRegion(shiftDetail, model);
                                }
                            }
                        }
                    }
                    if (!shiftCreated)
                    {
                        throw new Exception("Shift is already present");
                    }
                    
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
