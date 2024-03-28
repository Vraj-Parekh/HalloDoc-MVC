using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IRoleService
    {
        Task AddRole(CreateRoleDTO model);
        List<CreateRoleDTO> GetRoles();
    }
}