using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IAdminService
    {
        Task ChangePassword(Admin admin, AdminProfileDTO model);
        Admin GetAdmin(string email);
        AdminProfileDTO GetAdminInfo(Admin admin);
        Task UpdateAdminInfo(Admin admin, AdminProfileDTO model);
        Task UpdateBillingInfo(Admin admin, AdminProfileDTO model);
    }
}