using Entities.ViewModels;

namespace Repositories.Repository.Implementation
{
    public interface IOrderDetailsService
    {
        void AddOrderDetails(SendOrderDTO data, int requestId);
    }
}