using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class VendorsDTO
    {
        public string Profession { get; set; }
        public string BusinessName { get; set; }
        public string Email { get; set; }
        public string FaxNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string BusinessContact { get; set; }
        public int VendorId { get; set; }
    }
}
