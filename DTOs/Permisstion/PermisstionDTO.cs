using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestWebAPI.DTOs.Permisstion
{
    public class PermisstionDTO
    {
        [JsonIgnore]
        public string code { get; set; }

        [Required(ErrorMessage = "The 'value' field is required.")]
        public string value { get; set; }
    }
}
