using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class UserAccessDTO
    {
        public string AccountType { get; set; }
        public string AccountName { get; set; }
        public string Phone { get; set; }
        public int Status { get; set; }
        public int OpenRequests { get; set; }
        public int Id { get; set; }
    }
}
