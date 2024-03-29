using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IRoleMenuService
    {
        Task AddRoleMenus(int roleId, List<MenuDTO> menus);
        Task DeleteRoleMenus(int roleId);
    }
}