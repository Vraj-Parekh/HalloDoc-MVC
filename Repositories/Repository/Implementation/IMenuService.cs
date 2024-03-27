using Entities.Models;

namespace Repositories.Repository.Implementation
{
    public interface IMenuService
    {
        List<Menu> GetMenus(int accountType);
    }
}