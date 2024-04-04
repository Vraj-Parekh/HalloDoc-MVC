using Entities.Models;

namespace Repositories.Repository.Interface
{
    public interface IPhysicianNotificationService
    {
        Task<Physiciannotification> CreateNotification(Physician physician);
    }
}