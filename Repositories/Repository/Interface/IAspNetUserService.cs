using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IAspNetUserService
    {
        Task<Aspnetuser> AddAspNetUser(string email, string username, string phone, string password);
        string AuthenticateUser(LoginDTO data);
        void ChnagePassword(LoginDTO data);
        string GetAspNetUserId();
        bool isUserPresent(LoginDTO data);
    }
}