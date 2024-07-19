
using TestWebAPI.DTOs.Property;

namespace TestWebAPI.DTOs.Category
{
    public class GetCategoryDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string avatar { get; set; }
        public List<GetPropertyDTO> dataProperties { get; set; }
    }
}
