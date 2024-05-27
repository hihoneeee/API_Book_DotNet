using System.ComponentModel.DataAnnotations;

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
    }
    //public class RoleDTO
    //{
    ///[JsonIgnore]
    //public int id { get; set; }
    //[Required(ErrorMessage = "The 'value' field is required.")]
    //public string value { get; set; }
    //[JsonIgnore]
    //public string code { get; set; }
    //}
}
