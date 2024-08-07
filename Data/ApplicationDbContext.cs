﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TestWebAPI.Models;

namespace TestWebAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>().HasOne(p => p.category).WithMany(c => c.Properties).HasForeignKey(p => p.categoryId).HasPrincipalKey(c => c.id);
                        
            modelBuilder.Entity<User>().HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.roleCode).HasPrincipalKey(r => r.code);
            
            modelBuilder.Entity<JWT>().HasOne(j => j.user).WithMany(u => u.JWTs).HasForeignKey(j => j.userId).HasPrincipalKey(u => u.id);
            
            modelBuilder.Entity<User_Media>().HasOne(m => m.user).WithMany(u => u.User_Medias).HasForeignKey(m => m.userId).HasPrincipalKey(u => u.id);
            
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

            //PropertyHasDetail
            modelBuilder.Entity<PropertyHasDetail>()
                .HasOne(p => p.seller)
                .WithMany(u => u.PropertyHasDetails)
                .HasForeignKey(p => p.sellerId)
                .HasPrincipalKey(u => u.id);

            modelBuilder.Entity<PropertyHasDetail>()
                .HasOne(phd => phd.property)
                .WithOne(p => p.PropertyHasDetail)
                .HasForeignKey<PropertyHasDetail>(phd => phd.propertyId)
                .HasPrincipalKey<Property>(p => p.id);

            modelBuilder.Entity<PropertyHasDetail>()
                .Property(p => p.images)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<List<string>>(v));

            //nofication
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.user)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.buyerId)
                .HasPrincipalKey(u => u.id);

            //Evaluate
            modelBuilder.Entity<Evaluate>()
                .HasOne(e => e.contract)
                .WithOne(c => c.evaluate)
                .HasForeignKey<Evaluate>(e => e.contractId)
                .HasPrincipalKey<Contract>(c => c.id);

            //chat message
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m=> m.conversationId)
                .HasPrincipalKey(c => c.id);
            modelBuilder.Entity<Message>()
                .HasOne(m => m.User)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.userId)
                .HasPrincipalKey(u => u.id);

            //Wishlist
            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.buyer)
                .WithMany(u => u.Wishlists)
                .HasForeignKey(w => w.buyerId)
                .HasPrincipalKey(u => u.id);
            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.property)
                .WithMany(p => p.Wishlists)
                .HasForeignKey(w => w.propertyId)
                .HasPrincipalKey(p => p.id);

            //Appointment
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.buyer)
                .WithMany(u => u.Appointments)
                .HasForeignKey(a => a.buyerId)
                .HasPrincipalKey(u => u.id);
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.property)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.propertyId)
                .HasPrincipalKey(p => p.id);

            //Contract
            modelBuilder.Entity<Contract>()
                .HasOne(c => c.appointment)
                .WithOne(a => a.contract)
                .HasForeignKey<Contract>(c => c.appointmentId)
                .HasPrincipalKey<Appointment>(a => a.id);
                base.OnModelCreating(modelBuilder);

            //Payment
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.contract)
                .WithOne(c => c.payment)
                .HasForeignKey<Payment>(p => p.contractId)
                .HasPrincipalKey<Contract>(c => c.id);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<JWT> JWTs { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role_Permission> RolePermissions { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<User_Media> User_Medias { get; set; }
        public DbSet<Evaluate> Evaluates { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<PropertyHasDetail> PropertyHasDetails { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
