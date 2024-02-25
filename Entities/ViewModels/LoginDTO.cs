using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels
{
    public class LoginDTO
    {
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Password Not Match")]
        public string? ConfirmPassword { get; set; }
    }
}
