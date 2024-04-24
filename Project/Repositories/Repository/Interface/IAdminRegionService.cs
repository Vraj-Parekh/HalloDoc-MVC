using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IAdminRegionService
    {
        Task AddOrRemoveRegion(Admin admin, List<RegionList> model);
        List<Adminregion> GetAdminRegions(Admin admin);
    }
}