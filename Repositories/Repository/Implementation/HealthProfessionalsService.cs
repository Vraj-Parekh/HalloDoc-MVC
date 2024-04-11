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
using Twilio.Types;

namespace Repositories.Repository.Implementation
{
    public class HealthProfessionalsService: IHealthProfessionalsService
    {
        private readonly HalloDocDbContext _context;
        private readonly IRegionService regionService;

        public HealthProfessionalsService(HalloDocDbContext _context,IRegionService regionService)
        {
            this._context = _context;
            this.regionService = regionService;
        }

        public List<Healthprofessional> GetBusiness(int professionId)
        {
            List<Healthprofessional>? business = _context.Healthprofessionals.Where(a=>a.Profession == professionId).ToList();

            return business;
        }

        public async Task<List<VendorsDTO>> GetFilteredHealthProfessionals(string searchVendor,int profesionType)
        {
            IQueryable<Healthprofessional> query = _context.Healthprofessionals
                .Where(a=>a.Profession == profesionType || profesionType == 0)
                .Include(a=>a.ProfessionNavigation);

            if (!string.IsNullOrEmpty(searchVendor))
            {
                searchVendor = searchVendor.ToLower();
                query = query.Where(a => a.Vendorname.ToLower().Contains(searchVendor));
            }

            List<Healthprofessional>? healthprofessional = await query.ToListAsync();

            List<VendorsDTO> modelList = new List<VendorsDTO>();

            foreach(Healthprofessional item in healthprofessional)
            {
                VendorsDTO model = new VendorsDTO()
                {
                    Profession = item.ProfessionNavigation.Professionname,
                    BusinessName = item.Vendorname,
                    Email = item.Email,
                    FaxNumber = item.Faxnumber,
                    PhoneNumber = item.Phonenumber,
                    BusinessContact = item.Businesscontact,
                    VendorId = item.Vendorid,
                };
                modelList.Add(model);
            }

            return modelList;
        }

        public async Task<EditBusinessDTO> GetHealthProfessionalInfo(int vendorId)
        {
            Healthprofessional? vendor = _context.Healthprofessionals
                .Where(a => a.Vendorid == vendorId).FirstOrDefault();

            EditBusinessDTO model = new EditBusinessDTO();
            model.BusinessName = vendor.Vendorname;
            model.FaxNumber = vendor.Faxnumber;
            model.PhoneNumber = vendor.Phonenumber;
            model.Email = vendor.Email;
            model.BusinessContact = vendor.Businesscontact;
            model.Street = vendor.Address;
            model.City = vendor.City;
            model.Zip = vendor.Zip;
            model.Profession = (int)vendor.Profession;
            model.State = (int)vendor.Regionid;
            model.VendorId = vendorId;

            return model;
        }

        public async Task EditProfessional(int vendorId,EditBusinessDTO model)
        {
            Healthprofessional? vendor = _context.Healthprofessionals
               .Where(a => a.Vendorid == vendorId).FirstOrDefault();

            if(vendor != null)
            {
                vendor.Vendorname = model.BusinessName;
                vendor.Profession = model.Profession;
                vendor.Faxnumber = model.FaxNumber;
                vendor.Phonenumber = model.PhoneNumber;
                vendor.Address = model.Street;
                vendor.Email = model.Email;
                vendor.Businesscontact = model.BusinessContact;
                vendor.City = model.City;
                vendor.Regionid = model.State;
                vendor.State = regionService.GetRegionAbrName(model.State);
                vendor.Zip = model.Zip;

                _context.Update(vendor);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddBusiness(EditBusinessDTO model)
        {
            Healthprofessional healthProfessional = new Healthprofessional()
            {
                Address = model.Street,
                City = model.City,
                Businesscontact = model.BusinessContact,
                Createddate = DateTime.Now,
                Email = model.Email,
                Faxnumber = model.FaxNumber,
                Isdeleted = false,
                Phonenumber = model.PhoneNumber,
                Profession = model.Profession,
                Regionid = model.State,
                Vendorname = model.BusinessName,
                Zip = model.Zip,
                Vendorid = _context.Healthprofessionals.OrderBy(a => a.Vendorid).LastOrDefault().Vendorid + 1,
            };

            _context.Add(healthProfessional);
            await _context.SaveChangesAsync();
        }
    }
}
