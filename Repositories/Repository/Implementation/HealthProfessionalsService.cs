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
    public class HealthProfessionalsService: IHealthProfessionalsService
    {
        private readonly HalloDocDbContext _context;

        public HealthProfessionalsService(HalloDocDbContext _context)
        {
            this._context = _context;
        }

        public List<Healthprofessional> GetBusiness(int professionId)
        {
            List<Healthprofessional>? business = _context.Healthprofessionals.Where(a=>a.Profession == professionId).ToList();

            return business;
        }
    }
}
