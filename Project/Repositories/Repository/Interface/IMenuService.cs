using Entities.Models;

namespace Repositories.Repository.Interface
{
    public interface IMenuService
    {
        List<Menu> GetMenus(int accountType);
    }
}