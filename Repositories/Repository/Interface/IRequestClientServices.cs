using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IRequestClientServices
    {
        Requestclient GetClient(int requestId);
        void UpdateCase(ViewCaseDTO data);
    }
}