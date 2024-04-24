using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IEncounterFormService
    {
        void AddEncounterInfo(int requestId, EncounterDTO data);
        Task FinalizeRequest(int requestId);
        EncounterDTO GetEncounterInfo(int requestId);
        Task<bool> isFinalize(int requestId);
    }
}