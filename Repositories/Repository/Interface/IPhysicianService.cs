using Entities.Models;

namespace Repositories.Repository.Interface
{
    public interface IPhysicianService
    {
        List<Physician> GetPhysician(int regionId);
    }
}