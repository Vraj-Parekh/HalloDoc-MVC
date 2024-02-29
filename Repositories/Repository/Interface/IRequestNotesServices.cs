using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IRequestNotesServices
    {
        void AddNotes(ViewNotesDTO model,int requestId);
        Requestnote GetRequestNotes(int requestId);
        ViewNotesDTO GetViewRequestNotes(int requestId);
    }
}