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
    public class AspNetRoleService: IAspNetRoleService
    {
        private readonly HalloDocDbContext _context;

        public AspNetRoleService(HalloDocDbContext _context)
        {
            this._context = _context;
        }

        public Aspnetrole GetName(string name)
        {
            return _context.Aspnetroles.Where(r => r.Name == name).FirstOrDefault();
        }
    }
}
