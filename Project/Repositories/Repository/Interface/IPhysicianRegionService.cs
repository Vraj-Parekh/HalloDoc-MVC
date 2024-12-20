﻿using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IPhysicianRegionService
    {
        Task AddOrRemovePhysicianRegion(Physician physician, List<RegionList> regions);
        List<Physicianregion> GetPhysicianRegions(Physician physician);
        List<PhysicianRegionsDTO> GetRegions();
    }
}