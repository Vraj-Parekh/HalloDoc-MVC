using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class ProviderDashboardDTO
    {
        public int? RequestId { get; set; }
        public int RequestTypeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Phone { get; set; }
        public string? ClientPhone { get; set; }
        public string? Address { get; set; }
        public string Email { get; set; }
        public int? Status { get; set; }
        public int? CallType { get; set; }
    }
}
