using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class PatientRecordsDTO
    {
        public string ClientName { get; set; }
        public string CreatedDate { get; set; }
        public string Confirmation {  get; set; }
        public string ProviderName { get; set; }
        public string ConcludedDate { get; set; }
        public int Status { get; set; }
        public int RequestId { get; set; }
    }
}
