using Entities.Models;

namespace Repositories.Repository.Interface
{
    public interface IPhysicianLocationService
    {
        List<Physicianlocation> GetLocation();
    }
}