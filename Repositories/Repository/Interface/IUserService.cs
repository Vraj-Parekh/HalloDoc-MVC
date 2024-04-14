using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IUserService
    {
        Task<Pagination<PatientHistoryDTO>> GetFilteredUsers(string firstName, string lastName, string email, string phoneNumber, int page, int itemsPerPage);
        User GetUserByEmail(string email);
    }
}