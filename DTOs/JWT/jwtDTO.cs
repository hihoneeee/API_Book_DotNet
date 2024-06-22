using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.JWT
{
    public class jwtDTO
    {
        public string value { get; set; }
        public DateTime issuedDate { get; set; }
        public DateTime expiredDate { get; set; }
        public int userId { get; set; }
    }
}
