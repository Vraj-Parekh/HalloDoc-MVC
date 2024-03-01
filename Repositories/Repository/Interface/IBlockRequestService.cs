namespace Repositories.Repository.Interface
{
    public interface IBlockRequestService
    {
        void BlockRequest(int requestId, string reason);
    }
}