namespace Repositories.Repository.Interface
{
    public interface IRoleMenuService
    {
        Task AddRoleMenus(int roleId, List<int> menuIds);
    }
}