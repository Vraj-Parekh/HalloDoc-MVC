using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IRequestNotesServices
    {
        void AddNotes(int requestId);
        void AddNotes(ViewNotesDTO model);
        Requestnote GetRequestNotes(int requestId);
    }
}