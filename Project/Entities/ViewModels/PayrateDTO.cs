using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class PayrateDTO
    {
        public int Physicianid { get; set; }
        public int? NightShift_Weekend { get; set; }
        public int? Shift { get; set; }
        public int? HouseCalls_Nights_Weekend { get; set; }
        public int? HouseCalls { get; set; }
        public int? PhoneConsults_Nights_Weekend { get; set; }
        public int? PhoneConsults { get; set; }
        public int? BatchTesting { get; set; }
    }
}
