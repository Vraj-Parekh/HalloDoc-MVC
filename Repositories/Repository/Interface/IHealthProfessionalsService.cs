using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IHealthProfessionalsService
    {
        Task EditProfessional(int vendorId, EditBusinessDTO model);
        List<Healthprofessional> GetBusiness(int professionId);
        Task<EditBusinessDTO> GetHealthProfessionalInfo(int vendorId);
        Task<List<VendorsDTO>> GetHealthProfessionals(string searchVendor, int profesionType);
    }
}