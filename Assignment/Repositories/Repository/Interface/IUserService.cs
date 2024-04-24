using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IUserService
    {
        Task AddUserInfo(AddUserViewModel model);
        Task DeleteUser(int userId);
        Task EditUserInfo(int userId, AddUserViewModel model);
        Task<Pagination<UsersViewModel>> GetFilteredUsers(string search, int page, int itemsPerPage);
        Task<AddUserViewModel> GetUserInfo(int userId);
    }
}