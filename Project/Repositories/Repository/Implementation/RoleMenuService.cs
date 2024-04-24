using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Implementation
{
    public class RoleMenuService : IRoleMenuService
    {
        private readonly HalloDocDbContext _context;

        public RoleMenuService(HalloDocDbContext _context)
        {
            this._context = _context;
        }

        public async Task AddRoleMenus(int roleId, List<MenuDTO> menus)
        {
            List<Rolemenu>? roleMenuList = new List<Rolemenu>();
            foreach (MenuDTO menu in menus)
            {
                Rolemenu? roleMenu = new Rolemenu()
                {
                    Roleid = roleId,
                    Menuid = menu.MenuId,
                };
                roleMenuList.Add(roleMenu);
            }
            await _context.Rolemenus.AddRangeAsync(roleMenuList);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoleMenus(int roleId)
        {
            List<Rolemenu>? roleMenus = _context.Rolemenus.Where(a => a.Roleid == roleId).ToList();
            _context.Rolemenus.RemoveRange(roleMenus);
            await _context.SaveChangesAsync();
        }
    }
}
