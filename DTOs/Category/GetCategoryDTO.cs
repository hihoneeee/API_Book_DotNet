using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TestWebAPI.DTOs.Property;

namespace TestWebAPI.DTOs.Category
{
    public class GetCategoryDTO
    {
        [Required]
        public string title { get; set; }
        [Required]
        public long description { get; set; }
        [Required]
        public string avatar { get; set; }
        [JsonIgnore]
        public List<GetPropertyDTO> dataProperties { get; set; }

    }
}
