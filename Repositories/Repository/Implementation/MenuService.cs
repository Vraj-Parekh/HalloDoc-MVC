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
    public class MenuService: IMenuService
    {
        private readonly HalloDocDbContext _context;

        public MenuService(HalloDocDbContext _context)
        {
            this._context = _context;
        }

        public List<Menu> GetMenus(int accountType)
        {
            List<Menu>? menus = _context.Menus 
                .Where(a=>a.Accounttype == accountType)
                .Select(a=> new Menu { Menuid = a.Menuid, Name = a.Name})
                .ToList();

            return menus;
        }
    }
}
