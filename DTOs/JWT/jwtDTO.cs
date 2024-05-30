using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.JWT
{
    public class jwtDTO
    {
        public string value { get; set; }
        public DateTime issued_date { get; set; }
        public DateTime expired_date { get; set; }
        public int user_id { get; set; }
    }
}
