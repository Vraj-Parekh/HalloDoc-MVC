using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Implementation
{
    public class OrderDetailsService: IOrderDetailsService
    {
        private readonly HalloDocDbContext _context;

        public OrderDetailsService(HalloDocDbContext _context)
        {
            this._context = _context;
        }

        public void  AddOrderDetails(SendOrderDTO data, int requestId)
        {
            Orderdetail? orderDetail = new Orderdetail()
            {
                Requestid = requestId,
                Faxnumber = data.FaxNumber,
                Email = data.Email,
                Businesscontact = data.BusinessContact,
                Prescription = data.Prescription,
                Noofrefill = data.Refill,
                Createddate = DateTime.Now,
                Createdby = "admin",
                Vendorid = data.Business,
            };
            _context.Orderdetails.Add(orderDetail);
            _context.SaveChanges();
        }
    }
}