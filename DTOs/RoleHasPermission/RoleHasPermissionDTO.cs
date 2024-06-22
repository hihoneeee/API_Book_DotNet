using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.RoleHasPermission
{
    public class RoleHasPermissionDTO
    {
        [Required]
        public string codeRole { get; set; }

        [Required]
        public string codePermission { get; set; }
    }
}
