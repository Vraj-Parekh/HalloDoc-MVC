using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels 
{ 
    public class BusinessRequestDTO
    {

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^\+?\d+$", ErrorMessage = "Please enter a valid phone number")]
        public string Phone { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Business Name is required")]
        public string BusinessName { get; set; }

        public string CaseNumber { get; set; }
        public string? Symptoms { get; set; }
        [Required]
        public string PatientFirstName { get; set; }
        [Required]
        public string PatientLastName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string PatientEmail { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^\+?\d+$", ErrorMessage = "Please enter a valid phone number")]
        public string PatientPhone { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [RegularExpression(@"^\d{6}$", ErrorMessage = "ZIP code must be 6 digits")]
        public string ZipCode { get; set; }
        public string? RoomOrSuite { get; set; }
    }
}
