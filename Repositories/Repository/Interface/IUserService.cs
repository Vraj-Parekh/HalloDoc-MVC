using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IUserService
    {
        Task<List<PatientHistoryDTO>> GetFilteredUsers(string firstName, string lastName, string email, string phoneNumber);
        User GetUserByEmail(string email);
    }
}