using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IRequestClientServices
    {
        Requestclient GetClient(int requestId);
        void SendAgreement(int requestId, string phoneNumber, string email);
        void UpdateCase(ViewCaseDTO data);
    }
}