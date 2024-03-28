using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels
{
    public class CreateRoleDTO
    {
        [Required(ErrorMessage = "RoleName is required")]
        public string RoleName { get; set; }
        public int AccountType { get; set; }
        public List<int> Menus { get; set; }
        public int? RoleId { get; set; }
    }
}
