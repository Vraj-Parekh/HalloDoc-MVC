using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels
{
    public class ViewCaseDTO
    {
        public string? ConfirmationNumber { get; set; }
        public string PatientNotes { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? Region { get; set; }
        public string? BusinessName { get; set; }
        public string? Room { get; set; }
    }
}