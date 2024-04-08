using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NPOI.SS.Formula.Functions;
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
        private readonly IRegionService regionService;

        public PhysicianService(HalloDocDbContext _context, IAspNetUserService aspNetUserService, IAspNetRoleService aspNetRoleService, IAspNetUserRolesService aspNetUserRolesService,IPhysicianNotificationService physicianNotificationService,IPhysicianRegionService physicianRegionService,IRegionService regionService)
        {
            this._context = _context;
            this.aspNetUserService = aspNetUserService;
            this.aspNetRoleService = aspNetRoleService;
            this.aspNetUserRolesService = aspNetUserRolesService;
            this.physicianNotificationService = physicianNotificationService;
            this.physicianRegionService = physicianRegionService;
            this.regionService = regionService;
        }

        public Physician GetPhysicianById(int physicianId)
        {
            return _context.Physicians.Where(a => a.Physicianid == physicianId).FirstOrDefault();
        }
        public List<Physician> GetPhysicianByRegionId(int regionId)
        {
            List<Physician>? physician = _context.Physicians.Where(a => a.Regionid == regionId || regionId == 0).ToList();

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

        public EditPhysicianDTO GetPhysicianInfo(Physician physician)
        {
            EditPhysicianDTO model = new EditPhysicianDTO()
            {
                UserName = physician.Email,
                Status = (short)(physician.Status??0),
                Role = physician.Roleid??0,
                FirstName = physician.Firstname,
                LastName = physician.Lastname,
                Email = physician.Email,
                PhoneNumber = physician.Mobile,
                MedicalLicense = physician.Medicallicense,
                NPINumber = physician.Npinumber,
                SyncEmail = physician.Syncemailaddress,
                Address1 = physician.Address1,
                Address2 = physician.Address2,
                City = physician.City,
                State = (int)physician.Regionid,
                Zip = physician.Zip,
                AltPhoneNumber = physician.Altphone,
                BusinessName = physician.Businessname,
                BusinessWebsite = physician.Businesswebsite,
                AdminNotes = physician.Adminnotes,
                IsAgreementDoc = false,
                IsBackgroundDoc = false,
                IsNonDisclosureDoc = false,
                IsTrainingDoc = false,
            };

            List<Physicianregion>? physicianRegions = physicianRegionService.GetPhysicianRegions(physician);
            model.Regions = regionService.GetRegionList().Select(a => new RegionList()
            {
                RegionId = a.RegionId,
                IsPresent = physicianRegions.Any(b => b.Regionid == a.RegionId),
                RegionName = a.RegionName
            }).ToList();

            return model;
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
                Createddate = DateTime.Now,
                Createdby = aspNetUserService.GetAspNetUserId(),
                Isdeleted = false,
                Lastname = model.LastName,
                Medicallicense = model.MedicalLicense,
                Mobile = model.PhoneNumber,
                Npinumber = model.NPINumber,
                Regionid = model.State,
                Zip = model.Zip,
                Roleid = model.Role,
                Status = (short)RequestStatus.Pending,
                Photo = model.Photo.FileName,
                Signature = model.Signature.FileName,
                Physicianid = _context.Physicians.OrderBy(u => u.Physicianid).LastOrDefault().Physicianid + 1,
            };

            var file = model.Photo;
            var uniqueFileName = Path.GetFileName(file.FileName);
            var uploads = Path.Combine("wwwroot", "uploads");
            var filespath = Path.Combine(uploads, uniqueFileName);
            file.CopyTo(new FileStream(filespath, FileMode.Create));

            var file1 = model.Signature;
            var uniqueFileName1 = Path.GetFileName(file1.FileName);
            var uploads1 = Path.Combine("wwwroot", "uploads");
            var filespath1 = Path.Combine(uploads1, uniqueFileName1);
            file.CopyTo(new FileStream(filespath1, FileMode.Create));

            if (model.IsAgreementDoc != null && model.IsAgreementDoc.Length > 0)
            {
                string? newFileName = "Agreement.pdf";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", newFileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    model.IsAgreementDoc.CopyTo(stream);
                    physician.Isagreementdoc = true;
                }
            }


            if (model.IsBackgroundDoc != null && model.IsBackgroundDoc.Length > 0)
            {
                string? newFileName = "background.pdf";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", newFileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    model.IsBackgroundDoc.CopyTo(stream);
                    physician.Isbackgrounddoc = true;
                }
            }
            if (model.IsTrainingDoc != null && model.IsTrainingDoc.Length > 0)
            {
                string? newFileName = "HIPAA.pdf";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", newFileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    model.IsTrainingDoc.CopyTo(stream);
                    physician.Istrainingdoc = true;
                }
            }
            if (model.IsNonDisclosureDoc != null && model.IsNonDisclosureDoc.Length > 0)
            {
                string? newFileName = "NonDisclosure.pdf";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", newFileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    model.IsNonDisclosureDoc.CopyTo(stream);
                    physician.Isnondisclosuredoc = true;
                }
            }

            _context.Physicians.Add(physician);
            await _context.SaveChangesAsync();

            await physicianNotificationService.CreateNotification(physician);
            await physicianRegionService.AddOrRemovePhysicianRegion(physician, model.Regions);
        }
    }
}
