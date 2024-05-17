using Entities.Models;

namespace Repositories.Repository.Interface
{
    public interface IPayrateService
    {
        Payrate GetPayrate(int physicianid);
        Payrate SetPayrate(int Physicianid, int value, string paytype);
    }
}