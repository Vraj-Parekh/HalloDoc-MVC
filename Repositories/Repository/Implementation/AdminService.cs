﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using MathNet.Numerics.LinearAlgebra.Factorization;
using Microsoft.EntityFrameworkCore;
using Repositories.Repository.Interface;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Repositories.Repository.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly HalloDocDbContext _context;
        private readonly IAspNetUserService aspNetUserService;
        private readonly IAspNetRoleService aspNetRoleService;
        private readonly IAspNetUserRolesService aspNetUserRolesService;
        private readonly IAdminRegionService adminRegionService;
        private readonly IRegionService regionService;

        public AdminService(HalloDocDbContext _context,
                            IAspNetUserService aspNetUserService,
                            IAspNetRoleService aspNetRoleService,
                            IAspNetUserRolesService aspNetUserRolesService,
                            IAdminRegionService adminRegionService,
                            IRegionService regionService)
        {
            this._context = _context;
            this.aspNetUserService = aspNetUserService;
            this.aspNetRoleService = aspNetRoleService;
            this.aspNetUserRolesService = aspNetUserRolesService;
            this.adminRegionService = adminRegionService;
            this.regionService = regionService;
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
                Role = admin.Roleid ?? 0,
                FirstName = admin.Firstname,
                LastName = admin.Lastname,
                Email = admin.Email,
                ConfirmEmail = admin.Email,
                PhoneNumber = admin.Mobile,
                Address1 = admin.Address1,
                Address2 = admin.Address2,
                City = admin.City,
                State = (int)admin.Regionid,
                Zip = admin.Zip,
                AltPhoneNumber = admin.Altphone,
            };

            List<Adminregion>? adminRegions = adminRegionService.GetAdminRegions(admin);
            model.Regions = regionService.GetRegionList().Select(a => new RegionList()
            {
                RegionId = a.RegionId,
                IsPresent = adminRegions.Any(b => b.Regionid == a.RegionId),
                RegionName = a.RegionName
            }).ToList();


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

            await adminRegionService.AddOrRemoveRegion(admin, model.Regions);
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
            Aspnetuser? aspNetUser = await aspNetUserService.AddAspNetUser(model.Email, model.Email, model.PhoneNumber, model.Password);

            Aspnetrole? role = aspNetRoleService.GetName("Admin");

            if (role is not null && aspNetUser is not null)
            {
                //add into aspnet user roles table
                await aspNetUserRolesService.AddAspNetUserRole(aspNetUser, role);
            }

            //add admin 
            Admin admin = new Admin()
            {
                Address1 = model.Address1,
                Address2 = model.Address2,
                Altphone = model.AltPhoneNumber,
                City = model.City,
                Createdby = "Admin",
                Createddate = DateTime.Now,
                Email = model.Email,
                Firstname = model.FirstName,
                Isdeleted = false,
                Lastname = model.LastName,
                Mobile = model.PhoneNumber,
                Roleid = model.Role,
                Aspnetuserid = aspNetUser.Aspnetuserid,
                Regionid = model.State,
                Zip = model.Zip,
                Status = (short)RequestStatus.Pending,
            };

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            await adminRegionService.AddOrRemoveRegion(admin, model.Regions);
        }

        public async Task<Pagination<UserAccessDTO>> GetFilteredUserAccessData(int accountType, int page, int itemsPerPage)
        {
            List<UserAccessDTO> modelList = new List<UserAccessDTO>();

            if (accountType == 0)
            {
                var admins = await _context.Admins.ToListAsync();
                var physicians = await _context.Physicians.ToListAsync();

                foreach (var item in admins)
                {
                    modelList.Add(new UserAccessDTO
                    {
                        AccountName = item.Firstname + " " + item.Lastname,
                        AccountType = "Admin",
                        Id = item.Adminid,
                        OpenRequests = 0,//no idea
                        Phone = item.Mobile,
                        Status = (int)item.Status,
                    });
                }

                foreach (var item in physicians)
                {
                    modelList.Add(new UserAccessDTO
                    {
                        AccountName = item.Firstname + " " + item.Lastname,
                        AccountType = "Physician",
                        Id = item.Physicianid,
                        OpenRequests = 0,//no idea
                        Phone = item.Mobile,
                        Status = (int)item.Status,
                    });
                }
            }
            else if(accountType == 1)
            {
                var admins = await _context.Admins.ToListAsync();

                foreach (var item in admins)
                {
                    modelList.Add(new UserAccessDTO
                    {
                        AccountName = item.Firstname + " " + item.Lastname,
                        AccountType = "Admin",
                        Id = item.Adminid,
                        OpenRequests = 0,//no idea
                        Phone = item.Mobile,
                        Status = (int)item.Status,
                    });
                }
            }
            else
            {
                var physicians = await _context.Physicians.ToListAsync();
                foreach (var item in physicians)
                {
                    modelList.Add(new UserAccessDTO
                    {
                        AccountName = item.Firstname + " " + item.Lastname,
                        AccountType = "Physician",
                        Id = item.Physicianid,
                        OpenRequests = 0,//no idea
                        Phone = item.Mobile,
                        Status = (int)item.Status,
                    });
                }
            }

            int totalItems = modelList.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            if (page < 1) page = 1;

            int skip = (page - 1) * itemsPerPage;

            List<UserAccessDTO> paginatedUsers = modelList.Skip(skip)
                .Take(itemsPerPage)
                .ToList();

            return new Pagination<UserAccessDTO>
            {
                Data = paginatedUsers,
                TotalPages = totalPages,
                CurrentPage = page
            };
        }
    }
}
