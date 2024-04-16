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
        Task<MdOncallDTO> GetOnCallData(int regionId);
        Task<List<Shiftdetail>> GetShiftDetails();
        Task<List<Shiftdetail>> GetShiftsOnDate(DateTime date, int physicianId = 0);
    }
}