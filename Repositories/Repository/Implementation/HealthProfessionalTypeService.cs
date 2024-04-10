using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Implementation
{
    public class HealthProfessionalTypeService: IHealthProfessionalTypeService
    {
        private readonly HalloDocDbContext _context;

        public HealthProfessionalTypeService(HalloDocDbContext _context)
        {
            this._context = _context;
        }

        public List<Healthprofessionaltype> GetProfession()
        {
            List<Healthprofessionaltype>? Healthprofessionaltypes = _context.Healthprofessionaltypes.ToList();

            return Healthprofessionaltypes;
        }
    }
}
