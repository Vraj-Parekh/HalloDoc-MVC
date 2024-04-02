using Entities.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels
{
    public class CreatePhysicianDTO
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }
        public List<Role> Roles { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^\+?\d+$", ErrorMessage = "Please enter a valid phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address 1 is required")]
        public string Address1 { get; set; }

        public string? Address2 { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }
        public List<Region> Regions { get; set; }

        [Required(ErrorMessage = "Zip is required")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "ZIP code must be 6 digits")]
        public string Zip { get; set; }

        [RegularExpression(@"^\+?\d+$", ErrorMessage = "Please enter a valid phone number")]
        public string? AltPhoneNumber { get; set; }

        public string MedicalLicense { get; set; }

        public string NPINumber { get; set; }

        public string BusinessName { get; set; }

        public string BusinessWebsite { get; set; }

        public IFormFile Photo { get; set; }

        public string? AdminNotes { get; set; }

        public bool AgreementDoc {  get; set; }

        public bool BackgroundDoc {  get; set; }

        public bool TrainingDoc {  get; set; }

        public bool NonDisclosureDoc {  get; set; }
    }
}
