using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IHealthProfessionalTypeService
    {
        List<Healthprofessionaltype> GetProfession();
    }
}