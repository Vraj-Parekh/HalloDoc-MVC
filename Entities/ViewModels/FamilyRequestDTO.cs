using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels
{
    public class FamilyRequestDTO
    {

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        public string? RelationWithPatient { get; set; }
        public string? Symptoms { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string PatientEmail { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        public string PatientPhone { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [RegularExpression(@"^\d{6}$", ErrorMessage = "ZIP code must be 6 digits")]
        public string ZipCode { get; set; }
        public string? RoomOrSuite { get; set; }
        public List<IFormFile>? File { get; set; }
    }
}