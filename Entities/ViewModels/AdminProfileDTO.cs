using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace Entities.ViewModels
{
    public class AdminProfileDTO
    {
        public int AdminId {  get; set; }
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public int Status { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public int Role { get; set; }
        public List<Role> Roles { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Confirm Email is required")]
        [Compare("Email", ErrorMessage = "The email and confirmation email do not match")]
        public string ConfirmEmail { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^\+[0-9]{10,15}$", ErrorMessage = "Phone Number must start with '+' and be between 10 to 15 digits")]
        public string PhoneNumber { get; set; }

        [RegularExpression(@"^\+[0-9]{10,15}$", ErrorMessage = "Phone Number must start with '+' and be between 10 to 15 digits")]
        public string? AltPhoneNumber { get; set; }

        [Required(ErrorMessage = "Address 1 is required")]
        public string Address1 { get; set; }

        public string? Address2 { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        public int State { get; set; }
        public List<RegionList> Regions { get; set; }

        [Required(ErrorMessage = "Zip is required")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "ZIP code must be 6 digits")]
        public string Zip { get; set; }
    }
}
