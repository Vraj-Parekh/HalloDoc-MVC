using System.ComponentModel.DataAnnotations;

namespace HalloDoc_Project.DTO
{
    public class BusinessRequestDTO
    {

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
        public string BusinessName { get; set; }
        public string CaseNumber { get; set; }
        public string Symptoms { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string PatientEmail { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        public string PatientPhone { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string RoomOrSuite { get; set; }
    }
}
