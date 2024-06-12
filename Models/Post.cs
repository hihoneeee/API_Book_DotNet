using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebAPI.Models
{
    public class Post
    {
        public Guid id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string avatar {  get; set; }
        public string url { get; set; }
        public int user_id { get; set; }
        [ForeignKey("user_id")]
        public virtual User User { get; set; }
    }
}
