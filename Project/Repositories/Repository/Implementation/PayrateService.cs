using Entities.DataContext;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Implementation
{
    public class PayrateService: IPayrateService
    {
        private readonly HalloDocDbContext _context;

        public PayrateService(HalloDocDbContext _context)
        {
            this._context = _context;
        }

        public Payrate GetPayrate(int physicianid)
        {
            return _context.Payrates.FirstOrDefault(s => s.PhysicianId == physicianid);
        }
        public Payrate SetPayrate(int Physicianid, int value, string paytype)
        {
            Payrate p = _context.Payrates.FirstOrDefault(s => s.PhysicianId == Physicianid);
            if (p == null)
            {
                p = new Payrate();
                p.PhysicianId = Physicianid;
                p.Shift = 0;
                p.Housecall = 0;
                p.NightshiftWeekend = 0;
                p.HousecallnightWeekend = 0;
                p.BatchTesting = 0;
                p.PhoneConsult = 0;
                p.PhoneConsultNightWeekend = 0;
                _context.Payrates.Add(p);
                _context.SaveChanges();

            }
            else
            {
                if (paytype == "ShiftNightWeekend")
                    p.NightshiftWeekend = value;    
                if (paytype == "Shift")
                    p.Shift = value;    
                if (paytype == "HousecallNightWeekend")
                    p.HousecallnightWeekend = value;    
                if (paytype == "Housecall")
                    p.Housecall = value;   
                if (paytype == "PhoneConsultNightWeekend")
                    p.PhoneConsultNightWeekend = value;    
                if (paytype == "PhoneConsult")
                    p.PhoneConsult = value; 
                if (paytype == "BatchTesting")
                    p.BatchTesting = value;

                _context.Payrates.Update(p);
                _context.SaveChanges();
            }
            return p;
        }
    }
}
