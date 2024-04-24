using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class CreateShiftDTO
    {
        public int RegionId { get; set; }

        public int PhysicianId { get; set; }

        public DateTime ShiftDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public bool Repeat { get; set; }

        public List<int> Repeat_Days { get; set; }

        public int RepeatUpto { get; set; }
    }
}
