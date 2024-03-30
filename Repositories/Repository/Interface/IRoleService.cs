using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IRoleService
    {
        Task AddRole(CreateRoleDTO model);
        Task DeleteRole(int roleId);
        Task EditRole(int roleId, CreateRoleDTO model);
        List<AccountAccessDTO> GetAllRoles();
        CreateRoleDTO GetRole(int roleId);
        bool IsRolePresent(string roleName);
    }
}