using System.ComponentModel.DataAnnotations;

namespace HalloDoc_Project.DTO
{
    public class PatientRequestDTO
    {
        public string Symptoms { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        public string Street { get; set; }

        
        public string City { get; set; }

       
        public string State { get; set; }

        [DataType(DataType.PostalCode)]
        public string ZipCode { get; set; }

        public string RoomOrSuite { get; set; }

    }
}
