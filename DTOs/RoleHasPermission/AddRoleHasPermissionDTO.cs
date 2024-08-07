using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.RoleHasPermission
{
    public class AddRoleHasPermissionDTO
    {
        [Required]
        public string codeRole { get; set; }

        [Required]
        public List<string> codePermission { get; set; }
    }
}
