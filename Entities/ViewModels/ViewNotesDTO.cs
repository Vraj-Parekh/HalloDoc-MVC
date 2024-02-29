using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class ViewNotesDTO
    {
        public int RequestId { get; set; }
        public string? AdminNotes { get; set; }
        public string? AdditionalNotes { get; set; }
        public string? PhysicianNotes { get; set; }
        public List<string>? TransferNotes { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
    }
}
