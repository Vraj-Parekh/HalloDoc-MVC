using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using Repositories.Repository.Interface;

namespace Repositories.Repository.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly HalloDocDbContext _context;
        private readonly IAspNetUserService aspNetUserService;
        private readonly IAspNetRoleService aspNetRoleService;

        public AdminService(HalloDocDbContext _context,IAspNetUserService aspNetUserService,IAspNetRoleService aspNetRoleService)
        {
            this._context = _context;
            this.aspNetUserService = aspNetUserService;
            this.aspNetRoleService = aspNetRoleService;
        }

        public Admin GetAdmin(string email)
        {
            Admin? admin = _context.Admins
               .Where(a => a.Email == email)
               .Include(a => a.Aspnetuser)
               .FirstOrDefault();

            return admin;
        }
        public AdminProfileDTO GetAdminInfo(Admin admin)
        {
            AdminProfileDTO? model = new AdminProfileDTO()
            {
                UserName = admin.Aspnetuser.Username,
                Password = admin.Aspnetuser.Passwordhash,
                Status = (short)(admin.Status ?? 0),
                //Role = admin.
                FirstName = admin.Firstname,
                LastName = admin.Lastname,
                Email = admin.Email,
                ConfirmEmail = admin.Email,
                PhoneNumber = admin.Mobile,
                Address1 = admin.Address1,
                Address2 = admin.Address2,
                City = admin.City,
                //State
                Zip = admin.Zip,
                AltPhoneNumber = admin.Altphone,
            };
            return model;
        }

        public async Task ChangePassword(Admin admin, AdminProfileDTO model)
        {
            Aspnetuser? aspNetUser = await _context.Aspnetusers.FirstOrDefaultAsync(a => a.Aspnetuserid == admin.Aspnetuserid);
            if (aspNetUser is not null)
            {
                aspNetUser.Passwordhash = model.Password;
                _context.Aspnetusers.Update(aspNetUser);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAdminInfo(Admin admin, AdminProfileDTO model)
        {
            admin.Firstname = model.FirstName;
            admin.Lastname = model.LastName;
            admin.Email = model.Email;
            admin.Mobile = model.PhoneNumber;

            _context.Admins.Update(admin);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBillingInfo(Admin admin, AdminProfileDTO model)
        {
            admin.Address1 = model.Address1;
            admin.Address2 = model.Address2;
            admin.City = model.City;
            admin.Zip = model.Zip;
            admin.Altphone = model.AltPhoneNumber;
            admin.Regionid = model.State;

            _context.Admins.Update(admin);
            await _context.SaveChangesAsync();
        }

        public async Task CreateAdmin(CreateAdminDTO model)
        {
            Task<Aspnetuser>? aspNetUser = aspNetUserService.AddAspNetUser(model.Email, model.UserName, model.PhoneNumber, model.Password);

            var role = aspNetRoleService.GetName("Admin");

            if(role is not null)
            {
                //add into aspnet user roles table
            }
        }
    }
}
