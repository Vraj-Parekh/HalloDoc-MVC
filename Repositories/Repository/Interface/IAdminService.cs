using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IAdminService
    {
        Task ChangePassword(Admin admin, AdminProfileDTO model);
        Task CreateAdmin(CreateAdminDTO model);
        Admin GetAdmin(string email);
        AdminProfileDTO GetAdminInfo(Admin admin);
        Task<Pagination<UserAccessDTO>> GetFilteredUserAccessData(int accountType, int page, int itemsPerPage);
        Task UpdateAdminInfo(Admin admin, AdminProfileDTO model);
        Task UpdateBillingInfo(Admin admin, AdminProfileDTO model);
    }
}