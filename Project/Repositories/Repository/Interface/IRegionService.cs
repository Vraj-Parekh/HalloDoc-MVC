using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IRegionService
    {
        List<Region> GetRegion();
        string GetRegionAbrName(int regionId);
        List<RegionList> GetRegionList();
    }
}