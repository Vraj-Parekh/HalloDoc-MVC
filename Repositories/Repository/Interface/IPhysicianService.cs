using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IPhysicianService
    {
        Task ChangePassword(Physician physician, EditPhysicianDTO model);
        Task CreatePhysician(CreatePhysicianDTO model);
        Physician GetPhysician(string email);
        Physician GetPhysicianById(int physicianId);
        List<Physician> GetPhysicianByRegionId(int regionId);
        string GetPhysicianEmail(int physicianId);
        int GetPhysicianIdByAspNetUserId(string aspNetUserId);
        EditPhysicianDTO GetPhysicianInfo(Physician physician);
        string GetPhysicianPhone(int physicianId);
        List<ProviderMenuDTO> GetProviderMenu();
        Task UpdateBillingInfo(Physician physician, EditPhysicianDTO model);
        Task UpdatePhysicianInfo(Physician physician, EditPhysicianDTO model);
        Task UpdateProfileInfo(Physician physician, EditPhysicianDTO model);
    }
}