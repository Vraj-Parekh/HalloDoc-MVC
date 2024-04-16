using Entities.Models;

namespace Repositories.Repository.Interface
{
    public interface IRequestStatusLogServices
    {
        void AddCancelNote(int requestId, string reason, string notes);
        Task<Requeststatuslog> AddRequestStatusLogAsync(Request request, RequestStatus status);
        Task<Requeststatuslog> AddRequestStatusLogAsync(Request request, RequestStatus status, string message);
        Requeststatuslog GetTransferNotes(int requestId);
    }
}