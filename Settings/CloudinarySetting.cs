using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.Settings
{
    public class CloudinarySetting
    {
        [Required]
        public string apiName {  get; set; }
        [Required]
        public string apiKey { get; set; }
        [Required]
        public string apiSecret { get; set; }
    }
}
