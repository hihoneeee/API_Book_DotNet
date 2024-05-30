using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebAPI.Models
{
    public class JWT
    {
        [Key]
        public int id { get; set; }
        public string value { get; set; }
        public DateTime issued_date { get; set; }
        public DateTime expired_date { get; set; }
        public int user_id { get; set; }
        [ForeignKey("user_id")]
        public virtual User user { get; set; }
    }
}
