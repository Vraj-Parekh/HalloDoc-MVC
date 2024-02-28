using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IRequestServices
    {
        Task<bool> AgreeWithAgreementAsync(int requestId);
        List<AdminDashboardDTO> GetPatientdata(int requesttypeid,int status);
        Request? GetRequest(int requestId);
        ViewCaseDTO GetViewCase(int requestId);
        bool IsRequestPending(int requestId, string email);
        Task<bool> RejectAgreementAsync(int requestId, string message);
    }
}