using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestWebAPI.Helpers;

public enum StatusUser
{
    connect = 1,
    disconnect = 2
};

public enum TypeUser
{
    activity = 1,
    canncel = 2
};

namespace TestWebAPI.Models
{

    public class User
    {
        [Key]
        public int id { get; set; }
        [RegularExpression(RegexUtilities.EMAIL, ErrorMessage = "Email format is not Valid!")]
        [StringLength(70, MinimumLength = 5, ErrorMessage = "Email length must between 5 and 70 character")]
        public string? email { get; set; }
        [RegularExpression(RegexUtilities.PHONE_NUMBER, ErrorMessage = "Phone number is not valid format!")]
        [StringLength(11, MinimumLength = 9)]
        [Required]
        public string phone { get; set; }

        [StringLength(50, MinimumLength = 1, ErrorMessage = "First Name must contain at least 1 character and maximum to 50 character")]
        [Required]
        public string first_name { get; set; }

        [StringLength(10, MinimumLength = 1, ErrorMessage = "Last Name must contain at least 1 character and maximum to 10 character")]
        [Required]
        public string last_name { get; set; }

        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must contain atleast 8 character")]
        [Required]
        public string password { get; set; }
        public string? address { get; set; }
        public string? avatar { get; set; }
        [Required]
        public string roleCode { get; set; }
        [ForeignKey("roleCode")]
        public virtual Role Role { get; set; }
        public StatusUser status { get; set; } = StatusUser.disconnect;
        public TypeUser type { get; set; } = TypeUser.activity;
        public DateTime createdAt { get; set; } = DateTime.Now;
        public DateTime updatedAt { get; set; }
        public DateTime? passwordChangeAt { get; set; }
        public string? passwordResetToken { get; set; }
        public DateTime? passwordResetExpires { get; set; }
        public virtual ICollection<JWT>? JWTs { get; set; }
        public virtual ICollection<User_Media>? User_Medias { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }
        public virtual ICollection<Contract>? Contracts { get; set; }
        public virtual ICollection<Message>? Messages { get; set; }
        public virtual ICollection<Appointment>? Appointments { get; set; }
        public virtual ICollection<Wishlist>? Wishlists { get; set; }
        public virtual ICollection<PropertyHasDetail>? PropertyHasDetails { get; set; }
    }
}
