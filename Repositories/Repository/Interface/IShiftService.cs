using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IShiftService
    {
        Task<Shift> AddShift(Physician physician, CreateShiftDTO model);
    }
}