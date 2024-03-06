using Entities.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Implementation
{
    public class AspNetUserService
    {
        private readonly HalloDocDbContext _context;

        public AspNetUserService(HalloDocDbContext _context)
        {
            this._context = _context;
        }

        
    }
}
