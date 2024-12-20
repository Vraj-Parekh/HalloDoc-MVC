﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class ScheduleDTO
    {
        public int? Shiftid { get; set; }
        public int? ShiftDetailId { get; set; }
        public int Physicianid { get; set; }
        public string? PhysicianName { get; set; }
        public string? PhysicianPhoto { get; set; }
        public int Region { get; set; }
        public List<RegionList> Regions { get; set; }

        public DateOnly Startdate { get; set; }
        public DateTime Shiftdate { get; set; }
        public TimeOnly Starttime { get; set; }
        public TimeOnly Endtime { get; set; }

        public bool Isrepeat { get; set; }

        public string? checkWeekday { get; set; }

        public int? Repeatupto { get; set; }
        public short Status { get; set; }
        public List<ScheduleDTO> DayList { get; set; }
    }

    public class MdOncallDTO
    {
        public List<Physicianforcall> onCall { get; set; }
        public List<Physicianforcall> offDuty { get; set; }
    }
    public class Physicianforcall
    {
        public string PhysicianName { get; set; }
        public string Photo { get; set; }
    }

}
