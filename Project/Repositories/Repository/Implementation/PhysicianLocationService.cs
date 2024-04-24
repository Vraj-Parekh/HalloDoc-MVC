using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DataContext;
using Entities.Models;
using Repositories.Repository.Interface;

namespace Repositories.Repository.Implementation
{
    public class PhysicianLocationService: IPhysicianLocationService
    {
        private readonly HalloDocDbContext _context;

        public PhysicianLocationService(HalloDocDbContext _context)
        {
            this._context = _context;
        }

        public List<Physicianlocation> GetLocation()
        {
            List<Physicianlocation>? location = _context.Physicianlocations.ToList();
            return location;
        }
    }
}
