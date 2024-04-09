using Entities.DataContext;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Implementation
{
    public class SmsLogService: ISmsLogService
    {
        private readonly HalloDocDbContext _context;
        public SmsLogService(HalloDocDbContext _context)
        {
            this._context = _context;
        }
    }
}
