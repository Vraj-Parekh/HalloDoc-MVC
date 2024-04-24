using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels
{
    internal class ResetPasswordDTO
    {
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string UserName { get; set; }
    }
}