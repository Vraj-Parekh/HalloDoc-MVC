using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface ISmsLogService
    {
        Task AddSmsLog(string phoneNumber, string message, bool isSmsSent, Request request = null);
        Task<Pagination<LogsDTO>> GetFilteredSmsLogs(int role, string receiverName, string phoneNumber, DateTime createdDate, DateTime sentDate, int page, int itemsPerPage);
    }
}