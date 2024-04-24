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
    public class ShiftDetailRegionService: IShiftDetailRegionService
    {
        private readonly HalloDocDbContext _context;

        public ShiftDetailRegionService(HalloDocDbContext _context)
        {
            this._context = _context;
        }

        public async Task AddShiftDetailRegion(Shiftdetail shiftdetail , CreateShiftDTO model)
        {
            Shiftdetailregion shiftdetailregion = new Shiftdetailregion()
            {
                Isdeleted = false,
                Regionid = (int)shiftdetail.Regionid,
                Shiftdetailid = shiftdetail.Shiftdetailid,
            };
            _context.Add(shiftdetailregion);
            await _context.SaveChangesAsync();
        }
    }
}
