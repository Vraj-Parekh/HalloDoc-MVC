using Entities.DataContext;
using Entities.Models;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Implementation
{
    public class AspNetUserRolesService: IAspNetUserRolesService
    {
        private readonly HalloDocDbContext _context;

        public AspNetUserRolesService(HalloDocDbContext _context)
        {
            this._context = _context;
        }

        public async Task AddAspNetUserRole(Aspnetuser user,Aspnetrole role)
        {
            if (!user.Roles.Contains(role))
            {
                role.Users.Add(user);
                _context.Update(role);
                await _context.SaveChangesAsync();
            }
        }
    }
}
