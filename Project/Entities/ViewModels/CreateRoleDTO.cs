using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels
{
    public class CreateRoleDTO
    {
        [Required(ErrorMessage ="Role Name is required")]
        public string RoleName { get; set; }
        public int AccountType { get; set; }
        public List<MenuDTO> Menus { get; set; }
    }

    public class MenuDTO
    {
        public int MenuId { get; set; }
        public required string Name { get; set; }
        public bool IsPresent { get; set; }
    }
}
