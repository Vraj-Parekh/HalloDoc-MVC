using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IAspNetUserService
    {
        string AuthenticateUser(LoginDTO data);
    }
}