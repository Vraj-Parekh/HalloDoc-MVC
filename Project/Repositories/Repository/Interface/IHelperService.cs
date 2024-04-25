using Entities.Models;

namespace Repositories.Repository.Interface
{
    public interface IHelperService
    {
        Task<string> GetFileAsync(int fileId, int requestId);
        string GetLoggedinEmail();
        Physician GetPhysician();
        string GetRegionById(int regionId);
        List<string> GetRoles();
        bool IsAdmin();
        bool IsPhysician();
        Task SendAttachment(int request_id, int[] files_jx, string email);
    }
}