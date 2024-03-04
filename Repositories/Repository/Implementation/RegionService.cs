using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DataContext;
using Repositories.Repository.Interface;
using Entities.Models;

namespace Repositories.Repository.Implementation
{
    public class RegionService:IRegionService
    {
        private readonly HalloDocDbContext _context;

        public RegionService(HalloDocDbContext _context)
        {
            this._context = _context;
        }

        public List<Region> GetRegion()
        {
            List<Region>? regions = _context.Regions.ToList();

            return regions;
        }
    }
}
