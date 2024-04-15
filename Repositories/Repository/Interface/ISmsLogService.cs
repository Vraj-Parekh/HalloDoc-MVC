using Entities.Models;

namespace Repositories.Repository.Interface
{
    public interface ISmsLogService
    {
        void AddSmsLog(string phoneNumber, string message, Request request = null);
    }
}