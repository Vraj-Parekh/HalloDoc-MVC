using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IShiftDetailRegionService
    {
        Task AddShiftDetailRegion(Shiftdetail shiftdetail, CreateShiftDTO model);
    }
}