using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IOrderDetailsService
    {
        void AddOrderDetails(SendOrderDTO data, int requestId);
    }
}