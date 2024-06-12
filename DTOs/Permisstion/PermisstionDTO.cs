using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestWebAPI.DTOs.Permisstion
{
    public class PermisstionDTO
    {
        [Required(ErrorMessage = "The 'value' field is required.")]
        public string value { get; set; }
        [JsonIgnore]
        public string code { get; set; }

    }
}
