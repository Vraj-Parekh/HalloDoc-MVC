using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels
{
    public class PatientRequestDTO
    {
        public string? Symptoms { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^\+?\d+$", ErrorMessage = "Please enter a valid phone number")]
        public string PhoneNumber { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&@#])[A-Za-z\d@$!%*?&@#]{8,}$", ErrorMessage = "Invalid Password")]
        [Required]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password Not Match")]
        [Required(ErrorMessage = "Field missing")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [RegularExpression(@"^\d{6}$", ErrorMessage = "ZIP code must be 6 digits")]
        public string ZipCode { get; set; }

        public string? RoomOrSuite { get; set; }

        public List<IFormFile>? File { get; set; }
    }
}
