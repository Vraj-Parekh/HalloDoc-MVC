using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels
{
    public class ViewDocumentList
    {
        public int RequestId { get; set; }
        public List<FileData>? Document { get; set; }
        public string Name { get; set; }
        public string ConfirmationNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone Number must be 10 digits")]
        //[RegularExpression(@"^\+(?:[0-9] ?){6,14}[0-9]$", ErrorMessage = "Invalid mobile number format")]
        public string? PhoneNumber { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }
    }

    public class FileData
    {
        public string FileName { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int DocumentId { get; set; }
    }
}