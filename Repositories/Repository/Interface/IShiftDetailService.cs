using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IShiftDetailService
    {
        Task<Shiftdetail> AddShiftDetails(Shift shift, CreateShiftDTO model);
        Task ChangeShiftStatus(int shiftDetailId);
        Task DeleteShift(int shiftDetailId);
        Task EditShift(ScheduleDTO model);
        Task<List<Shiftdetail>> GetShiftDetails();
    }
}