using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TestWebAPI.Models;

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
