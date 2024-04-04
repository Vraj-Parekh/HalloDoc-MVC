﻿using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IRegionService
    {
        List<Region> GetRegion();
        List<RegionList> GetRegionList();
    }
}