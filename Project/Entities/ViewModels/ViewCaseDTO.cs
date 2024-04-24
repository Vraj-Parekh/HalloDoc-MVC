using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels
{
    public class ViewCaseDTO
    {
        public int RequestId { get; set; }
        public int RequestStatusType { get; set; }
        public string? ConfirmationNumber { get; set; }
        public string? PatientNotes { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^\+?\d+$", ErrorMessage = "Please enter a valid phone number")]
        public string PhoneNumber { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required]
        public string Region { get; set; }
        public string? BusinessName { get; set; }
        public string? Room { get; set; }
    }
}