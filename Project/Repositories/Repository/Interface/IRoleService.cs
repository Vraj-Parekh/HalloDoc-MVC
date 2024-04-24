using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IRoleService
    {
        Task AddRole(CreateRoleDTO model);
        Task DeleteRole(int roleId);
        Task EditRole(int roleId, CreateRoleDTO model);
        List<AccountAccessDTO> GetAllRoles();
        CreateRoleDTO GetRoleById(int roleId);
        List<Role> GetRoles();
        bool IsRolePresent(string roleName);
    }
}