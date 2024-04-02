using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IMenuService menuService;

        public RoleService(HalloDocDbContext _context, IRoleMenuService roleMenuService, IMenuService menuService)
        {
            this._context = _context;
            this.roleMenuService = roleMenuService;
            this.menuService = menuService;
        }

        public List<Role> GetRoles()
        {
             return _context.Roles.Where(r => !r.Isdeleted).ToList();
        }
        public bool IsRolePresent(string roleName)
        {
            return _context.Roles.Any(r => r.Name == roleName && !r.Isdeleted);
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

        public List<AccountAccessDTO> GetAllRoles()
        {
            List<Role>? roles = _context.Roles.ToList();
            List<AccountAccessDTO>? roleList = new List<AccountAccessDTO>();
            if (roles is not null)
            {
                foreach (Role role in roles)
                {
                    if (role.Isdeleted != true)
                    {
                        AccountAccessDTO model = new AccountAccessDTO();
                        model.RoleId = role.Roleid;
                        model.RoleName = role.Name;
                        model.AccountType = role.Accounttype;
                        roleList.Add(model);
                    }
                }
                return roleList;
            }
            return new List<AccountAccessDTO>();
        }

        public CreateRoleDTO GetRoleById(int roleId)
        {
            Role? role = _context.Roles.Where(a => a.Roleid == roleId).Include(a => a.Rolemenus).FirstOrDefault();

            if (role is not null)
            {
                List<MenuDTO>? menus = menuService.GetMenus(role.Accounttype).Select(a => new MenuDTO
                {
                    Name = a.Name,
                    IsPresent = role.Rolemenus.Any(b => a.Menuid == b.Menuid),
                    MenuId = a.Menuid
                }).ToList();


                CreateRoleDTO? model = new CreateRoleDTO()
                {
                    AccountType = role.Accounttype,
                    RoleName = role.Name,
                    Menus = menus,
                };
                return model;
            }
            return new CreateRoleDTO();
        }

        public async Task DeleteRole(int roleId)
        {
            Role? role = _context.Roles.Where(a => a.Roleid == roleId).FirstOrDefault();
            if (role is not null)
            {
                role.Isdeleted = true;
                _context.Roles.Update(role);
                await roleMenuService.DeleteRoleMenus(roleId);
                await _context.SaveChangesAsync();
            }
        }

        public async Task EditRole(int roleId, CreateRoleDTO model)
        {
            Role? role = _context.Roles.Where(a => a.Roleid == roleId).FirstOrDefault();

            if (role is not null)
            {
                role.Modifieddate = DateTime.Now;
                role.Modifiedby = "Admin";

                await roleMenuService.DeleteRoleMenus(roleId);

                List<MenuDTO>? menus = new List<MenuDTO>();

                foreach (MenuDTO item in model.Menus)
                {
                    if (item.IsPresent)
                    {
                        menus.Add(item);
                    }
                }
                await roleMenuService.AddRoleMenus(roleId, menus);
            }
        }
    }
}
