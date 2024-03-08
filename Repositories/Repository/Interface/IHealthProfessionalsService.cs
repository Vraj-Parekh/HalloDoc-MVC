using Entities.Models;

namespace Repositories.Repository.Interface
{
    public interface IHealthProfessionalsService
    {
        List<Healthprofessional> GetBusiness(int professionId);
    }
}