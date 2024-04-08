using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IShiftDetailService
    {
        Task<Shiftdetail> AddShiftDetails(Shift shift, CreateShiftDTO model);
        Task<List<Shiftdetail>> GetShiftDetails();
    }
}