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
            Aspnetuser? user = _context.Aspnetusers.Where(u=>u.Email == data.Email).Include(a=>a.Roles).Include(a=>a.Users).FirstOrDefault();

            if(user != null && user.Passwordhash == data.Password)
            {
                return JwtService.GenerateJwtToken(user);
            }
            else
            {
                return null;
            }
        }
        public bool isUserPresent(LoginDTO data)
        {
            return _context.Aspnetusers.Any(u => u.Email == data.Email);
        }
        public void ChnagePassword(LoginDTO data)
        {
            Aspnetuser? aspNetUserData = new Aspnetuser();
            aspNetUserData.Passwordhash = data.ConfirmPassword;

            _context.Aspnetusers.Update(aspNetUserData);
            _context.SaveChanges();
        }
    }
}
