using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class SearchRecordsDTO
    {
        public string PatientName { get; set; }
        public int Requestor { get; set; }
        public string DateofService { get; set; }
        public string CloseCaseDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public int RequestStatus { get; set; }
        public string Physician { get; set; }
        public string PhysicianNote { get; set; }
        public string CancelledByProviderNote { get; set; }
        public string AdminNote { get; set; }
        public string PatientNote { get; set; }
        public int RequestId { get; set; }
    }
}
