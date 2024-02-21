using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels
{
    public class PatientRequestDTO
    {
        public string? Symptoms { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password Not Match")]
        [Required(ErrorMessage="Field missing")]
        public string ConfirmPassword { get; set; }
        public string Street { get; set; }

        public string City { get; set; }


        public string State { get; set; }

        public string ZipCode { get; set; }

        public string? RoomOrSuite { get; set; }

        public List<IFormFile>? File { get; set; }
    }
}
