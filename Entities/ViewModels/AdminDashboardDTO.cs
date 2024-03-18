using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{

    public class AdminDashboardDTO
    {
        public int? RequestId { get; set; }
        public int RequestTypeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public string? PhysicianName { get; set; }
        public string? Requestor { get; set; }
        public DateTime RequestedDate { get; set; }
        public DateTime DateOfService { get; set; }
        public string? Region { get; set; }
        public string? Phone { get; set; }
        public string? ClientPhone { get; set; }
        public string? Physician { get; set; }
        public string? Address { get; set; }
        public string? Notes { get; set; }
        public string? ChatWith { get; set; }
        public int? Status { get; set; }
        public int? RegionId { get; set; }
        public string? Email { get; set; }
    }
}
