using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DataContext;
using Entities.ViewModels;
using Repositories.Repository.Interface;

namespace Repositories.Repository.Implementation
{
    public class AdminService:IAdminService
    {
        private readonly HalloDocDbContext _context;

        public AdminService(HalloDocDbContext _context)
        {
            this._context = _context;
        }

        //public AdminProfileDTO GetAdminProfileInfo(AdminProfileDTO model,int adminId)
        //{
        //    var admin = _context.Admins.Where(a=>a.Adminid == adminId).Include().FirstOrDefault();
        //}
    }
}
