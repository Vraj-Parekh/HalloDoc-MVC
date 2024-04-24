using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IHandleShiftService
    {
        Task AddShiftDetails(CreateShiftDTO model);
    }
}