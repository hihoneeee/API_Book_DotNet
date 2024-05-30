using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestWebAPI.Helpers;

namespace TestWebAPI.Models
{
    public class User
    {
        public int id { get; set; }
        [RegularExpression(RegexUtilities.EMAIL, ErrorMessage = "Email format is not Valid!")]
        [StringLength(70, MinimumLength = 5, ErrorMessage = "Email length must between 5 and 70 character")]
        public required string email { get; set; }
        [RegularExpression(RegexUtilities.PHONE_NUMBER, ErrorMessage = "Phone number is not valid format!")]
        [StringLength(11, MinimumLength = 9)]
        public string? phone { get; set; }

        [StringLength(50, MinimumLength = 1, ErrorMessage = "First Name must contain at least 1 character and maximum to 50 character")]
        public required string first_name { get; set; }

        [StringLength(10, MinimumLength = 1, ErrorMessage = "Last Name must contain at least 1 character and maximum to 10 character")]
        public required string last_name { get; set; }

        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must contain atleast 8 character")]
        public required string password { get; set; }
        public string? address { get; set; }
        public string? avatar { get; set; }

        [ForeignKey("Role")]
        public required string roleCode { get; set; }

        public virtual Role Role { get; set; }

        public virtual ICollection<JWT>? JWTs { get; set; }

    }
}
