using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class SendOrderDTO
    {
        //public int RequestId { get; set; }

        [Required(ErrorMessage = "Please select a profession.")]
        public string Profession { get; set; }

        [Required(ErrorMessage = "Please enter a business name.")]
        public int Business { get; set; }

        [Required(ErrorMessage = "Please enter a business contact.")]
        public string BusinessContact { get; set; }

        [Required(ErrorMessage = "Please enter an email.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a fax number.")]
        public string FaxNumber { get; set; }

        [Required(ErrorMessage = "Please enter prescription or order details.")]
        public string? Prescription { get; set; }

        public int Refill { get; set; }
    }
}
