using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly HalloDocDbContext _context;
        private readonly IRoleMenuService roleMenuService;

        public RoleService(HalloDocDbContext _context, IRoleMenuService roleMenuService)
        {
            this._context = _context;
            this.roleMenuService = roleMenuService;
        }

        public async Task AddRole(CreateRoleDTO model)
        {
            Role? role = new Role()
            {
                Name = model.RoleName,
                Accounttype = (short)model.AccountType,
                Createdby = "Admin",
                Createddate = DateTime.Now,
                //Modifiedby = "",
                Isdeleted = false,
            };
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            await roleMenuService.AddRoleMenus(role.Roleid, model.Menus);
        }

        public List<CreateRoleDTO> GetRoles()
        {
            List<Role>? roles = _context.Roles.ToList();
            List<CreateRoleDTO>? roleList = new List<CreateRoleDTO>();
            if (roles is not null)
            {
                foreach (Role role in roles)
                {
                    CreateRoleDTO model = new CreateRoleDTO();
                    model.RoleId = role.Roleid;
                    model.RoleName = role.Name;
                    model.AccountType = role.Accounttype;
                    roleList.Add(model);
                }
                return roleList;
            }
            return null;
        }
    }
}
