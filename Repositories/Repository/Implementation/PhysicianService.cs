using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Entities.DataContext;
using Entities.Models;
using Repositories.Repository.Interface;

namespace Repositories.Repository.Implementation
{
    public class PhysicianService:IPhysicianService
    {
        private readonly HalloDocDbContext _context;

        public PhysicianService(HalloDocDbContext _context)
        {
            this._context = _context;
        }

        public List<Physician> GetPhysician(int regionId)
        {
            List<Physician>? physician = _context.Physicians.Where(a=>a.Regionid == regionId).ToList();

            return physician;
        }
    }
}
