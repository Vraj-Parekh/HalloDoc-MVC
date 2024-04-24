using Entities.Models;

namespace Repositories.Repository.Interface
{
    public interface IAspNetUserRolesService
    {
        Task AddAspNetUserRole(Aspnetuser user, Aspnetrole role);
    }
}