using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IEmailLogService
    {
        Task AddEmailLog(string email, string message, string subject, bool isEmailSent, List<string>? attachments = null, Request request = null);
        Task<Pagination<LogsDTO>> GetFilteredEmailLogs(int role, string receiverName, string emailId, DateTime createdDate, DateTime sentDate, int page, int itemsPerPage);
    }
}