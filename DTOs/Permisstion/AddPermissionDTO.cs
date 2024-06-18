using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.Permisstion
{
    public class AddPermissionDTO
    {
        [Required(ErrorMessage = "The value field is required.")]
        public string value { get; set; }
    }
}
