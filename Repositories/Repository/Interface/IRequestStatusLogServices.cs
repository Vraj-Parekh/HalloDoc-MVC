using Entities.Models;

namespace Repositories.Repository.Interface
{
    public interface IRequestStatusLogServices
    {
        Task<Requeststatuslog> AddRequestStatusLogAsync(Request request, RequestStatus status);
        Task<Requeststatuslog> AddRequestStatusLogAsync(Request request, RequestStatus status, string message);
        Requeststatuslog? GetTransferNotes(int requestId);
    }
}