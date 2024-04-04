using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using Repositories.Repository.Interface;

namespace Repositories.Repository.Implementation
{
    public class PhysicianService : IPhysicianService
    {
        private readonly HalloDocDbContext _context;
        private readonly IAspNetUserService aspNetUserService;
        private readonly IAspNetRoleService aspNetRoleService;
        private readonly IAspNetUserRolesService aspNetUserRolesService;
        private readonly IPhysicianNotificationService physicianNotificationService;
        private readonly IPhysicianRegionService physicianRegionService;

        public PhysicianService(HalloDocDbContext _context, IAspNetUserService aspNetUserService, IAspNetRoleService aspNetRoleService, IAspNetUserRolesService aspNetUserRolesService,IPhysicianNotificationService physicianNotificationService,IPhysicianRegionService physicianRegionService)
        {
            this._context = _context;
            this.aspNetUserService = aspNetUserService;
            this.aspNetRoleService = aspNetRoleService;
            this.aspNetUserRolesService = aspNetUserRolesService;
            this.physicianNotificationService = physicianNotificationService;
            this.physicianRegionService = physicianRegionService;
        }

        public List<Physician> GetPhysicianByRegionId(int regionId)
        {
            List<Physician>? physician = _context.Physicians.Where(a => a.Regionid == regionId).ToList();

            return physician;
        }

        public List<ProviderMenuDTO> GetProviderMenu()
        {
            List<Physician>? providerList = _context.Physicians
                .Include(a => a.Physiciannotifications)
                .ToList();

            List<ProviderMenuDTO> model = new List<ProviderMenuDTO>();

            if (providerList is not null)
            {
                foreach (Physician physician in providerList)
                {
                    ProviderMenuDTO providerMenuData = new ProviderMenuDTO()
                    {
                        ProviderName = physician.Firstname,
                        PhysicianId = physician.Physicianid,
                        Status = physician.Status,
                    };
                    model.Add(providerMenuData);
                }

            }
            return model;
        }

        public string GetPhysicianEmail(int physicianId)
        {
            string? email = _context.Physicians
                .Where(a => a.Physicianid == physicianId)
                .Select(a => a.Email)
                .FirstOrDefault();
            return email;
        }

        public string GetPhysicianPhone(int physicianId)
        {
            string? mobile = _context.Physicians
                .Where(a => a.Physicianid == physicianId)
                .Select(a => a.Mobile)
                .FirstOrDefault();
            return mobile;
        }

        public async Task CreatePhysician(CreatePhysicianDTO model)
        {
            Aspnetuser? aspNetUser = await aspNetUserService.AddAspNetUser(model.Email, model.Email, model.PhoneNumber, model.Password);

            Aspnetrole? role = aspNetRoleService.GetName("Admin");

            if (role is not null && aspNetUser is not null)
            {
                //add into aspnet user roles table
                await aspNetUserRolesService.AddAspNetUserRole(aspNetUser, role);
            }

            Physician physician = new Physician()
            {
                //Aspnetuserid = aspNetUser.Aspnetuserid,
                Aspnetuser=aspNetUser,
                Address1 = model.Address1,
                Address2 = model.Address2,
                Adminnotes = model.AdminNotes,
                Altphone = model.AltPhoneNumber,
                Businessname = model.BusinessName,
                Businesswebsite = model.BusinessWebsite,
                City = model.City,
                Email = model.Email,
                Firstname = model.FirstName,
                Isagreementdoc = model.IsAgreementDoc,
                Createddate = DateTime.Now,
                //Createdby = "Admin",
                Isbackgrounddoc = model.IsBackgroundDoc,
                Isdeleted = false,
                Lastname = model.LastName,
                Medicallicense = model.MedicalLicense,
                Mobile = model.PhoneNumber,
                Npinumber = model.NPINumber,
                Regionid = model.State,
                Zip = model.Zip,
                Isnondisclosuredoc = model.IsNonDisclosureDoc,
                Istrainingdoc = model.IsTrainingDoc,
                Roleid = model.Role,
                Status = (short)RequestStatus.Pending,
                Physicianid = _context.Physicians.OrderBy(u => u.Physicianid).LastOrDefault().Physicianid + 1,
            };

            _context.Add(physician);
            await _context.SaveChangesAsync();

            await physicianNotificationService.CreateNotification(physician);
            await physicianRegionService.AddOrRemovePhysicianRegion(physician, model.Regions);
        }
    }
}
