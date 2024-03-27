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
    public class PhysicianService:IPhysicianService
    {
        private readonly HalloDocDbContext _context;

        public PhysicianService(HalloDocDbContext _context)
        {
            this._context = _context;
        }

        public List<Physician> GetPhysicianByRegionId(int regionId)
        {
            List<Physician>? physician = _context.Physicians.Where(a=>a.Regionid == regionId).ToList();

            return physician;
        }

        public List<ProviderMenuDTO> GetProviderMenu()
        {
            List<Physician>? providerList = _context.Physicians
                .Include(a=>a.Physiciannotifications)
                .ToList();

            List<ProviderMenuDTO> model = new List<ProviderMenuDTO>();
            
            if (providerList is not null)
            {
                foreach(Physician physician in providerList)
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
                .Where(a=>a.Physicianid == physicianId)
                .Select(a=>a.Email)
                .FirstOrDefault();
            return email;
        }      
        
        public string GetPhysicianPhone(int physicianId)
        {
            string? mobile = _context.Physicians
                .Where(a=>a.Physicianid == physicianId)
                .Select(a=>a.Mobile)
                .FirstOrDefault();
            return mobile;
        }
    }
}
