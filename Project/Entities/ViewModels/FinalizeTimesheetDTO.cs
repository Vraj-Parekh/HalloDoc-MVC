using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class FinalizeTimesheetDTO
    {
        public string selectedvalue { get; set; }
        public List<Finalizetimesheettime> phytimesheet { get; set; }

        public List<Addreciepts> Reciepts { get; set; }

        public int id { get; set; }

        public bool isfinalize { get; set; }

        public bool isapproved { get; set; }

        public bool issubmitted { get; set; }

        public int physicianid { get; set; }
    }

    public class Finalizetimesheettime
    {
        public DateOnly date { get; set; }

        public int oncallhours { get; set; }

        public int totalhours { get; set; }

        public bool holiday { get; set; }

        public int noofhousecalls { get; set; }

        public int noofphoneconsult { get; set; }
    }

    public class Addreciepts
    {
        public int id { get; set; }

        public DateOnly date { get; set; }

        public string Item { get; set; }

        public int Amount { get; set; }

        public string Bill { get; set; }

        public IFormFile? Upload { get; set; }

    }
}

