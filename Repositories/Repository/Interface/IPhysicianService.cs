using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IPhysicianService
    {
        Task CreatePhysician(CreatePhysicianDTO model);
        Physician GetPhysician(string email);
        Physician GetPhysicianById(int physicianId);
        List<Physician> GetPhysicianByRegionId(int regionId);
        string GetPhysicianEmail(int physicianId);
        EditPhysicianDTO GetPhysicianInfo(Physician physician);
        string GetPhysicianPhone(int physicianId);
        List<ProviderMenuDTO> GetProviderMenu();
    }
}