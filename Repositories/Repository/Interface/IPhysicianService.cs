using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IPhysicianService
    {
        Task CreatePhysician(CreatePhysicianDTO model);
        List<Physician> GetPhysicianByRegionId(int regionId);
        string GetPhysicianEmail(int physicianId);
        string GetPhysicianPhone(int physicianId);
        List<ProviderMenuDTO> GetProviderMenu();
    }
}