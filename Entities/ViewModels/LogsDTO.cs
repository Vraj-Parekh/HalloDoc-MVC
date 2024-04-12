using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class LogsDTO
    {
        public string Recipient { get; set; }
        public int Active { get; set; }
        public int Role { get; set; }
        public List<Role> Roles { get; set; }
        public string Email { get; set; }
        public string CreatedDate { get; set; }
        public string SentDate { get; set; }
        public string Sent {  get; set; }
        public int SentTries { get; set; }
        public string ConfirmationNumber { get; set; }
    }
}
