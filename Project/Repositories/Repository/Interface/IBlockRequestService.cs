using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IBlockRequestService
    {
        void BlockRequest(int requestId, string reason);
        Task<Pagination<BlockHistoryDTO>> GetFilteredBlockedHistry(string name, DateTime createdDate, string email, string phonenumber, int page, int itemsPerPage);
        Task UnblockRequest(int requestId);
    }
}