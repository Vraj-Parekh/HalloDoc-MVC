using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class RequestedExport
    {
        public String Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Requestor { get; set; }
        public DateTime RequestedDate { get; set; }
        public string RequestType { get; set; }
        public string Notes { get; set; }
        public string address { get; set; }
    }
}
