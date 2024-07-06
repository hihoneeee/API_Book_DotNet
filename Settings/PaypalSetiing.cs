using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.Settings
{
    public class PaypalSetiing
    {
        [Required]
        public string appId { get; set; }
        [Required]
        public string appSecret { get; set; }
        [Required]
        public string mode { get; set; }
    }
}
