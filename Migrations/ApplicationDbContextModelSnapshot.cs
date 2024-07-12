﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestWebAPI.Data;

#nullable disable

namespace TestWebAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TestWebAPI.Models.Appointment", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<DateTime>("appointmentDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("backupDay")
                        .HasColumnType("datetime2");

                    b.Property<int>("buyerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("propertyId")
                        .HasColumnType("int");

                    b.Property<int>("status")
                        .HasColumnType("int");

                    b.Property<DateTime>("updatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("buyerId");

                    b.HasIndex("propertyId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("TestWebAPI.Models.Category", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("avatar")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("createAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("updateAt")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("TestWebAPI.Models.Contract", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("EarnestMoney")
                        .HasColumnType("int");

                    b.Property<int?>("Propertyid")
                        .HasColumnType("int");

                    b.Property<int?>("Userid")
                        .HasColumnType("int");

                    b.Property<int>("appointmentId")
                        .HasColumnType("int");

                    b.Property<string>("content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("signatureBuyer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("signatureSeller")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("updatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("Propertyid");

                    b.HasIndex("Userid");

                    b.HasIndex("appointmentId")
                        .IsUnique();

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("TestWebAPI.Models.Conversation", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("userId1")
                        .HasColumnType("int");

                    b.Property<int>("userId2")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("Conversations");
                });

            modelBuilder.Entity("TestWebAPI.Models.Evaluate", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("buyerId")
                        .HasColumnType("int");

                    b.Property<int>("contractId")
                        .HasColumnType("int");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("review")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("star")
                        .HasColumnType("int");

                    b.Property<DateTime>("updatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("contractId")
                        .IsUnique();

                    b.ToTable("Evaluates");
                });

            modelBuilder.Entity("TestWebAPI.Models.JWT", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<DateTime>("expiredDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("issuedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.Property<string>("value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("userId");

                    b.ToTable("JWTs");
                });

            modelBuilder.Entity("TestWebAPI.Models.Message", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("conversationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("conversationId");

                    b.HasIndex("userId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("TestWebAPI.Models.Notification", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int?>("Conversationid")
                        .HasColumnType("int");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<int?>("Propertyid")
                        .HasColumnType("int");

                    b.Property<int>("buyerId")
                        .HasColumnType("int");

                    b.Property<string>("content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("updatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("Conversationid");

                    b.HasIndex("Propertyid");

                    b.HasIndex("userId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("TestWebAPI.Models.Payment", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("contractId")
                        .HasColumnType("int");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("paypalId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("contractId")
                        .IsUnique();

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("TestWebAPI.Models.Permission", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("code")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("updatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("TestWebAPI.Models.Property", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("avatar")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("categoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("price")
                        .HasColumnType("int");

                    b.Property<int>("status")
                        .HasColumnType("int");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("updatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("categoryId");

                    b.ToTable("Properties");
                });

            modelBuilder.Entity("TestWebAPI.Models.PropertyHasDetail", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("bathroom")
                        .HasColumnType("int");

                    b.Property<int>("bedroom")
                        .HasColumnType("int");

                    b.Property<string>("city")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("images")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("propertyId")
                        .HasColumnType("int");

                    b.Property<string>("province")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("sellerId")
                        .HasColumnType("int");

                    b.Property<int>("size")
                        .HasColumnType("int");

                    b.Property<int>("type")
                        .HasColumnType("int");

                    b.Property<int>("yearBuild")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("propertyId")
                        .IsUnique();

                    b.HasIndex("sellerId");

                    b.ToTable("PropertyHasDetails");
                });

            modelBuilder.Entity("TestWebAPI.Models.Role", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("code")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("createAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("updateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("value")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("TestWebAPI.Models.Role_Permission", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("codePermission")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("codeRole")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("id");

                    b.HasIndex("codePermission");

                    b.HasIndex("codeRole");

                    b.ToTable("RolePermissions");
                });

            modelBuilder.Entity("TestWebAPI.Models.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<string>("first_name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("last_name")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("passwordChangeAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("passwordResetExpires")
                        .HasColumnType("datetime2");

                    b.Property<string>("passwordResetToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phone")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("roleCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("id");

                    b.HasIndex("roleCode");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TestWebAPI.Models.User_Media", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("icon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("link")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("provider")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("updatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("userId");

                    b.ToTable("User_Medias");
                });

            modelBuilder.Entity("TestWebAPI.Models.Wishlist", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("buyerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("propertyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("updatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("buyerId");

                    b.HasIndex("propertyId");

                    b.ToTable("Wishlists");
                });

            modelBuilder.Entity("TestWebAPI.Models.Appointment", b =>
                {
                    b.HasOne("TestWebAPI.Models.User", "buyer")
                        .WithMany("Appointments")
                        .HasForeignKey("buyerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestWebAPI.Models.Property", "property")
                        .WithMany("Appointments")
                        .HasForeignKey("propertyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("buyer");

                    b.Navigation("property");
                });

            modelBuilder.Entity("TestWebAPI.Models.Contract", b =>
                {
                    b.HasOne("TestWebAPI.Models.Property", null)
                        .WithMany("Contracts")
                        .HasForeignKey("Propertyid");

                    b.HasOne("TestWebAPI.Models.User", null)
                        .WithMany("Contracts")
                        .HasForeignKey("Userid");

                    b.HasOne("TestWebAPI.Models.Appointment", "appointment")
                        .WithOne("contract")
                        .HasForeignKey("TestWebAPI.Models.Contract", "appointmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("appointment");
                });

            modelBuilder.Entity("TestWebAPI.Models.Evaluate", b =>
                {
                    b.HasOne("TestWebAPI.Models.Contract", "contract")
                        .WithOne("evaluate")
                        .HasForeignKey("TestWebAPI.Models.Evaluate", "contractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("contract");
                });

            modelBuilder.Entity("TestWebAPI.Models.JWT", b =>
                {
                    b.HasOne("TestWebAPI.Models.User", "user")
                        .WithMany("JWTs")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("TestWebAPI.Models.Message", b =>
                {
                    b.HasOne("TestWebAPI.Models.Conversation", "Conversation")
                        .WithMany("Messages")
                        .HasForeignKey("conversationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestWebAPI.Models.User", "User")
                        .WithMany("Messages")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Conversation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TestWebAPI.Models.Notification", b =>
                {
                    b.HasOne("TestWebAPI.Models.Conversation", null)
                        .WithMany("Notifications")
                        .HasForeignKey("Conversationid");

                    b.HasOne("TestWebAPI.Models.Property", null)
                        .WithMany("Notifications")
                        .HasForeignKey("Propertyid");

                    b.HasOne("TestWebAPI.Models.User", "user")
                        .WithMany("Notifications")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("TestWebAPI.Models.Payment", b =>
                {
                    b.HasOne("TestWebAPI.Models.Contract", "contract")
                        .WithOne("payment")
                        .HasForeignKey("TestWebAPI.Models.Payment", "contractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("contract");
                });

            modelBuilder.Entity("TestWebAPI.Models.Property", b =>
                {
                    b.HasOne("TestWebAPI.Models.Category", "category")
                        .WithMany("Properties")
                        .HasForeignKey("categoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("category");
                });

            modelBuilder.Entity("TestWebAPI.Models.PropertyHasDetail", b =>
                {
                    b.HasOne("TestWebAPI.Models.Property", "property")
                        .WithOne("PropertyHasDetail")
                        .HasForeignKey("TestWebAPI.Models.PropertyHasDetail", "propertyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestWebAPI.Models.User", "seller")
                        .WithMany("PropertyHasDetails")
                        .HasForeignKey("sellerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("property");

                    b.Navigation("seller");
                });

            modelBuilder.Entity("TestWebAPI.Models.Role_Permission", b =>
                {
                    b.HasOne("TestWebAPI.Models.Permission", "Permission")
                        .WithMany("Role_Permissions")
                        .HasForeignKey("codePermission")
                        .HasPrincipalKey("code")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestWebAPI.Models.Role", "Role")
                        .WithMany("Role_Permissions")
                        .HasForeignKey("codeRole")
                        .HasPrincipalKey("code")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("TestWebAPI.Models.User", b =>
                {
                    b.HasOne("TestWebAPI.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("roleCode")
                        .HasPrincipalKey("code")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("TestWebAPI.Models.User_Media", b =>
                {
                    b.HasOne("TestWebAPI.Models.User", "user")
                        .WithMany("User_Medias")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("TestWebAPI.Models.Wishlist", b =>
                {
                    b.HasOne("TestWebAPI.Models.User", "buyer")
                        .WithMany("Wishlists")
                        .HasForeignKey("buyerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestWebAPI.Models.Property", "property")
                        .WithMany("Wishlists")
                        .HasForeignKey("propertyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("buyer");

                    b.Navigation("property");
                });

            modelBuilder.Entity("TestWebAPI.Models.Appointment", b =>
                {
                    b.Navigation("contract")
                        .IsRequired();
                });

            modelBuilder.Entity("TestWebAPI.Models.Category", b =>
                {
                    b.Navigation("Properties");
                });

            modelBuilder.Entity("TestWebAPI.Models.Contract", b =>
                {
                    b.Navigation("evaluate")
                        .IsRequired();

                    b.Navigation("payment")
                        .IsRequired();
                });

            modelBuilder.Entity("TestWebAPI.Models.Conversation", b =>
                {
                    b.Navigation("Messages");

                    b.Navigation("Notifications");
                });

            modelBuilder.Entity("TestWebAPI.Models.Permission", b =>
                {
                    b.Navigation("Role_Permissions");
                });

            modelBuilder.Entity("TestWebAPI.Models.Property", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Contracts");

                    b.Navigation("Notifications");

                    b.Navigation("PropertyHasDetail")
                        .IsRequired();

                    b.Navigation("Wishlists");
                });

            modelBuilder.Entity("TestWebAPI.Models.Role", b =>
                {
                    b.Navigation("Role_Permissions");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("TestWebAPI.Models.User", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Contracts");

                    b.Navigation("JWTs");

                    b.Navigation("Messages");

                    b.Navigation("Notifications");

                    b.Navigation("PropertyHasDetails");

                    b.Navigation("User_Medias");

                    b.Navigation("Wishlists");
                });
#pragma warning restore 612, 618
        }
    }
}
