using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Entities.DataContext;
using Entities.Enum;
using Entities.Models;
using Entities.ViewModels;
using HalloDoc.Utility;
using MailKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NPOI.SS.Formula.Eval;
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

        public PhysicianService(HalloDocDbContext _context, IAspNetUserService aspNetUserService, IAspNetRoleService aspNetRoleService, IAspNetUserRolesService aspNetUserRolesService, IPhysicianNotificationService physicianNotificationService, IPhysicianRegionService physicianRegionService, IRegionService regionService)
        {
            this._context = _context;
            this.aspNetUserService = aspNetUserService;
            this.aspNetRoleService = aspNetRoleService;
            this.aspNetUserRolesService = aspNetUserRolesService;
            this.physicianNotificationService = physicianNotificationService;
            this.physicianRegionService = physicianRegionService;
            this.regionService = regionService;
        }

        public Physician GetPhysician(string email)
        {
            var physician = _context.Physicians
               .Where(a => a.Email == email)
               .FirstOrDefault();

            return physician;
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

        public int GetPhysicianIdByAspNetUserId(string aspNetUserId)
        {
            return _context.Physicians.Where(a => a.Aspnetuserid == aspNetUserId).Select(a => a.Physicianid).FirstOrDefault();
        }
        public async Task<Pagination<ProviderMenuDTO>> GetProviderMenu(int regionId, int page, int itemsPerPage)
        {
            IQueryable<Physician>? query = _context.Physicians
                .Include(a => a.Physiciannotifications)
                .AsQueryable();


            if (regionId > 0)
            {
                query = query.Where(a => a.Regionid == regionId);
            }

            int totalItems = await query.CountAsync();


            if (page < 1) page = 1;

            int skip = (page - 1) * itemsPerPage;
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            List<Physician>? pysicianList = await query
                .Skip(skip)
                .Take(itemsPerPage)
                .ToListAsync();

            List<ProviderMenuDTO> modelList = new List<ProviderMenuDTO>();


            foreach (Physician physician in pysicianList)
            {
                var rolename = _context.Roles.Where(a => a.Roleid == physician.Roleid).Select(a=>a.Name).FirstOrDefault();

                ProviderMenuDTO providerMenuData = new ProviderMenuDTO()
                {
                    ProviderName = physician.Firstname,
                    PhysicianId = physician.Physicianid,
                    Status = physician.Status,
                    Role = rolename
                };
                modelList.Add(providerMenuData);
            }

            return new Pagination<ProviderMenuDTO>
            {
                Data = modelList,
                TotalPages = totalPages,
                CurrentPage = page
            };
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
            EditPhysicianDTO model = new EditPhysicianDTO();

            model.UserName = physician.Email;
            model.Status = physician.Status ?? 0;
            model.Role = physician.Roleid ?? 0;
            model.FirstName = physician.Firstname;
            model.LastName = physician.Lastname;
            model.Email = physician.Email;
            model.PhoneNumber = physician.Mobile;
            model.MedicalLicense = physician.Medicallicense;
            model.NPINumber = physician.Npinumber;
            model.SyncEmail = physician.Syncemailaddress;
            model.Address1 = physician.Address1;
            model.Address2 = physician.Address2;
            model.City = physician.City;
            model.State = (int)physician.Regionid;
            model.Zip = physician.Zip;
            model.AltPhoneNumber = physician.Altphone;
            model.BusinessName = physician.Businessname;
            model.BusinessWebsite = physician.Businesswebsite;
            model.IsAgreementDoc = physician.Isagreementdoc;
            model.IsBackgroundDoc = physician.Isbackgrounddoc;
            model.IsNonDisclosureDoc = physician.Isnondisclosuredoc;
            model.IsTrainingDoc = physician.Istrainingdoc;
            model.PhotoImg = physician.Photo;
            model.SignImg = physician.Signature;
            model.PhysicianId = physician.Physicianid;

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

            Aspnetrole? role = aspNetRoleService.GetName("Provider");

            if (role is not null && aspNetUser is not null)
            {
                //add into aspnet user roles table
                await aspNetUserRolesService.AddAspNetUserRole(aspNetUser, role);
            }

            Physician physician = new Physician()
            {
                //Aspnetuserid = aspNetUser.Aspnetuserid,
                Aspnetuser = aspNetUser,
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
                Status = (short)UserStatus.Pending,
                Physicianid = _context.Physicians.OrderBy(u => u.Physicianid).LastOrDefault().Physicianid + 1,
            };

            if (model.Photo != null)
            {
                physician.Photo = model.Photo.FileName;
                var uniqueFileName = Path.GetFileName(model.Photo.FileName);
                var uploads = Path.Combine("wwwroot", "uploads");
                var filespath = Path.Combine(uploads, uniqueFileName);
                model.Photo.CopyTo(new FileStream(filespath, FileMode.Create));
            }

            if (model.Signature != null)
            {
                physician.Signature = model.Signature.FileName;
                var uniqueFileName1 = Path.GetFileName(model.Signature.FileName);
                var uploads1 = Path.Combine("wwwroot", "uploads");
                var filespath1 = Path.Combine(uploads1, uniqueFileName1);
                model.Signature.CopyTo(new FileStream(filespath1, FileMode.Create));
            }
          
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

        public async Task ChangePassword(Physician physician, EditPhysicianDTO model)
        {
            Aspnetuser? aspNetUser = await _context.Aspnetusers.FirstOrDefaultAsync(a => a.Aspnetuserid == physician.Aspnetuserid);
            if (aspNetUser is not null)
            {
                aspNetUser.Passwordhash = model.Password;
                physician.Status = (short)model.Status;
                physician.Roleid = model.Role;

                _context.Aspnetusers.Update(aspNetUser);
                _context.Physicians.Update(physician);
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdatePhysicianInfo(Physician physician, EditPhysicianDTO model)
        {
            Aspnetuser? aspNetUser = await _context.Aspnetusers.FirstOrDefaultAsync(a => a.Aspnetuserid == physician.Aspnetuserid);

            if (aspNetUser is not null)
            {
                physician.Firstname = model.FirstName;
                physician.Lastname = model.LastName;
                physician.Email = model.Email;
                physician.Mobile = model.PhoneNumber;
                physician.Medicallicense = model.MedicalLicense;
                physician.Npinumber = model.NPINumber;

                aspNetUser.Email = model.Email;

                _context.Physicians.Update(physician);
                _context.Aspnetusers.Update(aspNetUser);
                await _context.SaveChangesAsync();

                await physicianRegionService.AddOrRemovePhysicianRegion(physician, model.Regions);
            }
        }

        public async Task UpdateBillingInfo(Physician physician, EditPhysicianDTO model)
        {
            physician.Address1 = model.Address1;
            physician.Address2 = model.Address2;
            physician.City = model.City;
            physician.Zip = model.Zip;
            physician.Altphone = model.AltPhoneNumber;
            physician.Regionid = model.State;

            _context.Physicians.Update(physician);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProfileInfo(Physician physician, EditPhysicianDTO model)
        {
            physician.Businessname = model.BusinessName;
            physician.Businesswebsite = model.BusinessWebsite;
            physician.Adminnotes = model.AdminNotes;

            if (model.Photo != null)
            {
                physician.Photo = model.Photo.FileName;
                var uniqueFileName = Path.GetFileName(model.Photo.FileName);
                var uploads = Path.Combine("wwwroot", "uploads");
                var filespath = Path.Combine(uploads, uniqueFileName);
                model.Photo.CopyTo(new FileStream(filespath, FileMode.Create));
            }

            if (model.Signature != null)
            {
                physician.Signature = model.Signature.FileName;
                var uniqueFileName1 = Path.GetFileName(model.Signature.FileName);
                var uploads1 = Path.Combine("wwwroot", "uploads");
                var filespath1 = Path.Combine(uploads1, uniqueFileName1);
                model.Signature.CopyTo(new FileStream(filespath1, FileMode.Create));
            }

            _context.Physicians.Update(physician);
            await _context.SaveChangesAsync();
        }
    }
}
