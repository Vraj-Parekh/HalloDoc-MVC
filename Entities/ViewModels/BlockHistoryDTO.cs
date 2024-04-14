using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class BlockHistoryDTO
    {
        public string PatientName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email {  get; set; }
        public string CreatedDate { get; set; }
        public string Notes { get; set; }
        public int RequestId { get; set; }
        public bool IsActive { get; set; }
    }
}
