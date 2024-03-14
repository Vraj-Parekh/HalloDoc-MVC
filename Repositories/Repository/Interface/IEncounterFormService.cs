using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IEncounterFormService
    {
        EncounterDTO GetEncounterInfo(int requestId);
    }
}