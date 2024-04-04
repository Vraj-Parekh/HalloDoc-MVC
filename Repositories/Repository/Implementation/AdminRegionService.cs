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
    public class AdminRegionService: IAdminRegionService
    {
        private readonly HalloDocDbContext _context;

        public AdminRegionService(HalloDocDbContext _context)
        {
            this._context = _context;
        }

        public async Task AddOrRemoveRegion(Admin admin,List<RegionList> model)
        {
            foreach (RegionList item in model)
            {
                Adminregion? region = admin.Adminregions.FirstOrDefault(a => a.Regionid == item.RegionId);
                if (region is not null && !item.IsPresent)
                {
                    _context.Remove(region);
                }
                else if (region is null && item.IsPresent)
                {
                    Adminregion adminregion = new Adminregion()
                    {
                        Adminid = admin.Adminid,
                        Regionid = item.RegionId
                    };
                    _context.Add(adminregion);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
