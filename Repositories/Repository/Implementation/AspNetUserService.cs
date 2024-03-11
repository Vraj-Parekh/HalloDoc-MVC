using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Implementation
{
    public class AspNetUserService :IAspNetUserService
    {
        private readonly HalloDocDbContext _context;

        public AspNetUserService(HalloDocDbContext _context)
        {
            this._context = _context;
        }

        public string AuthenticateUser(LoginDTO data)
        {
            Aspnetuser? user = _context.Aspnetusers.Where(u=>u.Email == data.Email).Include(a=>a.Roles).FirstOrDefault();

            if(user != null && user.Passwordhash == data.Password)
            {
                return JwtService.GenerateJwtToken(user);
            }
            else
            {
                return null;
            }
        }
    }
}
