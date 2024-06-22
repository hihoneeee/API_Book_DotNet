using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebAPI.Models
{
    public class JWT
    {
        [Key]
        public int id { get; set; }
        public string value { get; set; }
        public DateTime issuedDate { get; set; }
        public DateTime expiredDate { get; set; }
        public int userId { get; set; }
        [ForeignKey("userId")]
        public virtual User user { get; set; }
    }
}
