using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class EditBusinessDTO
    {
        public int VendorId { get; set; }

        [Required(ErrorMessage = "Business Name is required")]
        public string BusinessName {  get; set; }

        public string? FaxNumber { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^\+?\d+$", ErrorMessage = "Please enter a valid phone number")]
        public string PhoneNumber { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public string? BusinessContact { get; set; }

        public string? Street { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        public int State { get; set; }

        public List<RegionList> Regions { get; set; }

        [Required(ErrorMessage = "Zip is required")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "ZIP code must be 6 digits")]
        public string Zip { get; set; }

        [Required(ErrorMessage = "Profession is required")]
        public int Profession { get; set; }
        public List<Healthprofessionaltype> ProfessionList { get; set; }
    }
}
