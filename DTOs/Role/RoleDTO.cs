using System.ComponentModel.DataAnnotations;
using TestWebAPI.DTOs.Permisstion;

namespace TestWebAPI.DTOs.Role
{
    public class AddRoleDTO
    {
        [Required(ErrorMessage = "The 'value' field is required.")]
        public string value { get; set; }
    }
    public class RoleDTO
    {        
        public int id { get; set; }
        public string value { get; set; }
        public string code { get; set; }
        public List<PermisstionDTO> dataPermission { get; set; }
    }
}
