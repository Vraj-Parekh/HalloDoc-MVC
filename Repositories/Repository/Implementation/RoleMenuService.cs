using Entities.DataContext;
using Entities.Models;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Implementation
{
    public class RoleMenuService: IRoleMenuService
    {
        private readonly HalloDocDbContext _context;

        public RoleMenuService(HalloDocDbContext _context)
        {
            this._context = _context;
        }

        public async Task AddRoleMenus(int roleId,List<int> menuIds)
        {
            List<Rolemenu>? roleMenuList = new List<Rolemenu>();
            foreach (int id in menuIds)
            {
                Rolemenu? roleMenu = new Rolemenu()
                {
                    Roleid = roleId,
                    Menuid = id,
                };
                roleMenuList.Add(roleMenu);
            }
            await _context.SaveChangesAsync();
        }
    }
}
