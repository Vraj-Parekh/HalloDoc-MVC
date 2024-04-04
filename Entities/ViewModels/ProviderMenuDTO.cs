using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class ProviderMenuDTO
    {
        public bool? Notification{ get; set; }
        public string? ProviderName { get; set; }
        public string? Role {  get; set; }
        public string? OnCallStatus { get; set; }
        public int? Status { get; set; }
        public int? PhysicianId { get; set; }
        public int State { get; set; }
        public List<RegionList> Regions { get; set; }
    }
}
