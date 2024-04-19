using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using NuGet.Protocol.Core.Types;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public List<Adminregion> GetAdminRegions(Admin admin)
        {
             return _context.Adminregions.Where(a=>a.Adminid == admin.Adminid).ToList();
        }
        public async Task AddOrRemoveRegion(Admin admin,List<RegionList> model)
        {
            var adminRegions = GetAdminRegions(admin);

            foreach (var item in adminRegions)
            {
                _context.Remove(item);

            }
            foreach (RegionList item in model)
            {
                Adminregion adminregion= new()
                {
                    Adminid = admin.Adminid,
                    Regionid = item.RegionId
                };
                _context.Add(adminregion);
            }
            await _context.SaveChangesAsync();
        }
    }
}
