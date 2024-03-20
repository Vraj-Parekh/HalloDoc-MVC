using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IEncounterFormService
    {
        void AddEncounterInfo(int requestId, EncounterDTO data);
        EncounterDTO GetEncounterInfo(int requestId);
    }
}