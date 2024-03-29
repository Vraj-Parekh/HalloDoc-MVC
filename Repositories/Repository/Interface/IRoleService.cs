using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IRoleService
    {
        Task AddRole(CreateRoleDTO model);
        Task DeleteRole(int roleId);
        List<AccountAccessDTO> GetAllRoles();
        CreateRoleDTO GetRole(int roleId);
    }
}