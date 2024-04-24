using Entities.DataContext;
using Entities.Models;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Implementation
{
    public class PhysicianNotificationService: IPhysicianNotificationService
    {
        private readonly HalloDocDbContext _context;

        public PhysicianNotificationService(HalloDocDbContext _context)
        {
            this._context = _context;
        }

        public async Task<Physiciannotification> CreateNotification(Physician physician)
        {
            if(physician is not null)
            {
                Physiciannotification physiciannotification = new Physiciannotification()
                {
                    Isnotificationstopped = false,
                    Physicianid = physician.Physicianid
                };
                _context.Add(physiciannotification);
                await _context.SaveChangesAsync();

                return physiciannotification;
            }
            return null;
        }
    }
}
