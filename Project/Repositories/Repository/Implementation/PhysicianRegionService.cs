﻿using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Implementation
{
    public class PhysicianRegionService : IPhysicianRegionService
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
            var physicianRegions = GetPhysicianRegions(physician);

            foreach (var item in physicianRegions)
            {
                _context.Remove(item);

            }
            foreach (RegionList item in regions)
            {
                Physicianregion physicianregion = new()
                {
                    Physicianid = physician.Physicianid,
                    Regionid = item.RegionId
                };
                _context.Add(physicianregion);
            }
            await _context.SaveChangesAsync();
        }
    }
}