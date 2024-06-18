using Microsoft.EntityFrameworkCore;
using TestWebAPI.Models;

namespace TestWebAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>().HasOne(p => p.category).WithMany(c => c.Properties).HasForeignKey(p => p.category_id).HasPrincipalKey(c => c.id);
            
            modelBuilder.Entity<Property>().HasOne(p => p.seller).WithMany(u => u.Properties).HasForeignKey(p => p.seller_id).HasPrincipalKey(u => u.id);
            
            modelBuilder.Entity<User>().HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.roleCode).HasPrincipalKey(r => r.code);
            
            modelBuilder.Entity<JWT>().HasOne(j => j.user).WithMany(u => u.JWTs).HasForeignKey(j => j.user_id).HasPrincipalKey(u => u.id);
            
            modelBuilder.Entity<User_Media>().HasOne(m => m.user).WithMany(u => u.User_Medias).HasForeignKey(m => m.user_id).HasPrincipalKey(u => u.id);
            
            //Role has permission
            modelBuilder.Entity<Role_Permission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.Role_Permissions)
                .HasForeignKey(rp => rp.codeRole)
                .HasPrincipalKey(r => r.code);

            modelBuilder.Entity<Role_Permission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.Role_Permissions)
                .HasForeignKey(rp => rp.codePermission)
                .HasPrincipalKey(p => p.code);

            //nofication
            modelBuilder.Entity<Nofication>()
                .HasOne(n => n.property)
                .WithMany(p => p.Nofications)
                .HasForeignKey(n => n.property_id)
                .HasPrincipalKey(p => p.id);
            modelBuilder.Entity<Nofication>()
                .HasOne(n => n.user)
                .WithMany(u => u.Nofications)
                .HasForeignKey(n => n.user_id)
                .HasPrincipalKey(u => u.id);

            //Evaluate
            modelBuilder.Entity<Evaluate>()
                .HasOne(e => e.buyer)
                .WithMany(u => u.Evaluates)
                .HasForeignKey(e => e.buyer_id)
                .HasPrincipalKey(u => u.id);
            modelBuilder.Entity<Evaluate>()
                .HasOne(e => e.property)
                .WithMany(p => p.Evaluates)
                .HasForeignKey(e => e.property_id)
                .HasPrincipalKey(p => p.id);

            //Evaluate
            modelBuilder.Entity<Contract>()
                .HasOne(c => c.buyer)
                .WithMany(u => u.Contracts)
                .HasForeignKey(c => c.buyer_id)
                .HasPrincipalKey(u => u.id);
            modelBuilder.Entity<Contract>()
                .HasOne(c => c.property)
                .WithMany(p => p.Contracts)
                .HasForeignKey(c => c.property_id)
                .HasPrincipalKey(p => p.id);

            //Submission
            modelBuilder.Entity<Submission>()
                .HasOne(s => s.buyer)
                .WithMany(u => u.Submissions)
                .HasForeignKey(s => s.buyer_id)
                .HasPrincipalKey(u => u.id);
            modelBuilder.Entity<Submission>()
                .HasOne(s => s.property)
                .WithMany(p => p.Submissions)
                .HasForeignKey(s => s.property_id)
                .HasPrincipalKey(p => p.id);

            //Appointment
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.buyer)
                .WithMany(u => u.Appointments)
                .HasForeignKey(a => a.buyer_id)
                .HasPrincipalKey(u => u.id);
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.property)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.property_id)
                .HasPrincipalKey(p => p.id);

            //Wishlist
            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.buyer)
                .WithMany(u => u.Wishlists)
                .HasForeignKey(w => w.buyer_id)
                .HasPrincipalKey(u => u.id);
            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.property)
                .WithMany(p => p.Wishlists)
                .HasForeignKey(w => w.property_id)
                .HasPrincipalKey(p => p.id);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<JWT> JWTs { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role_Permission> RolePermissions { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Nofication> Nofications { get; set; }
        public DbSet<User_Media> User_Medias { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Evaluate> Evaluates { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }

    }
}
