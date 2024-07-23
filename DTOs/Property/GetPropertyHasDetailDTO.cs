using System.ComponentModel.DataAnnotations;
using TestWebAPI.DTOs.User;

namespace TestWebAPI.DTOs.Property
{
    public class GetPropertyHasDetailDTO
    {
        public int id { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public List<string> images { get; set; }
        public string address { get; set; }
        public int bedroom { get; set; }
        public int bathroom { get; set; }
        public int yearBuild { get; set; }
        public int size { get; set; }
        public GetUserDTO userData { get; set; }
        public int propertyId { get; set; }
        public statusEnum status { get; set; }
    }
}
