namespace Repositories.Repository.Interface
{
    public interface IRequestServices
    {
        Task<bool> AgreeWithAgreementAsync(int requestId);
        bool IsRequestPending(int requestId, string email);
        Task<bool> RejectAgreementAsync(int requestId, string message);
    }
}