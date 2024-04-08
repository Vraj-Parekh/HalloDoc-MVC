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
    public class PhysicianRegionService: IPhysicianRegionService
    {
        private readonly HalloDocDbContext _context;

        public PhysicianRegionService(HalloDocDbContext _context)
        {
            this._context = _context;
        }

        public List<Physicianregion> GetPhysicianRegions(Physician physician)
        {
            return _context.Physicianregions.Where(a => a.Physicianid == physician.Physicianid).ToList();
        }

        public async Task AddOrRemovePhysicianRegion(Physician physician, List<RegionList> regions)
        {
            foreach (RegionList item in regions)
            {
                Physicianregion? region = physician.Physicianregions.FirstOrDefault(a => a.Regionid == item.RegionId);
                if (region is not null && !item.IsPresent)
                {
                    _context.Remove(region);
                }
                else if (region is null && item.IsPresent)
                {
                    Physicianregion physicianregion = new()
                    {
                        Physicianid = physician.Physicianid,
                        Regionid = item.RegionId
                    };
                    _context.Add(physicianregion);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
