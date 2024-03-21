using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class ViewNotesDTO
    {
        public string? AdminNotes { get; set; }

        [Required(ErrorMessage ="Notes required")]
        public string AdditionalNotes { get; set; }

        public string? PhysicianNotes { get; set; }
        public List<string>? TransferNotes { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
    }
}
