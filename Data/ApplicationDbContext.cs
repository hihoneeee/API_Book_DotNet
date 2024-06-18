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
            
            modelBuilder.Entity<Property>().HasOne(p => p.user).WithMany(u => u.Properties).HasForeignKey(p => p.owner).HasPrincipalKey(u => u.id);
            
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

            //comment
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

    }
}
