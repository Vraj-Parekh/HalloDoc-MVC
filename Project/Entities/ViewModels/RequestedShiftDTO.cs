using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class RequestedShiftDTO
    {
        public string Staff {  get; set; }
        public string Day { get; set; }
        public string Time {  get; set; }
        public string RegionName { get; set; }
        public int ShiftDetailId { get; set; }
    }
}
