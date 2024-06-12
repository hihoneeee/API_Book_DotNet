using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.Models
{
    public class Role
    {
        public int Id { get; set; }
        [StringLength(50)]
        public required string code { get; set; }
        [StringLength(100)]
        public required string value { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; }
        public virtual ICollection<User>? Users { get; set; }
        public virtual ICollection<Role_Permission>? Role_Permissions { get; set; }

    }
}
