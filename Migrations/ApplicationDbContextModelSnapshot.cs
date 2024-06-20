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

                    b.Property<DateTime>("appointment_date")
                        .HasColumnType("datetime2");

                    b.Property<int>("buyer_id")
                        .HasColumnType("int");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("is_active")
                        .HasColumnType("bit");

                    b.Property<int>("property_id")
                        .HasColumnType("int");

                    b.Property<DateTime>("secondary_day")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("buyer_id");

                    b.HasIndex("property_id");

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

                    b.Property<long>("description")
                        .HasColumnType("bigint");

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

                    b.Property<int?>("Propertyid")
                        .HasColumnType("int");

                    b.Property<int?>("Userid")
                        .HasColumnType("int");

                    b.Property<int>("appointment_id")
                        .HasColumnType("int");

                    b.Property<string>("content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("signature_buyer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("signature_seller")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("Propertyid");

                    b.HasIndex("Userid");

                    b.HasIndex("appointment_id");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("TestWebAPI.Models.Evaluate", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("buyer_id")
                        .HasColumnType("int");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("property_id")
                        .HasColumnType("int");

                    b.Property<string>("review")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("star")
                        .HasColumnType("int");

                    b.Property<DateTime>("updatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("buyer_id");

                    b.HasIndex("property_id");

                    b.ToTable("Evaluates");
                });

            modelBuilder.Entity("TestWebAPI.Models.JWT", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<DateTime>("expired_date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("issued_date")
                        .HasColumnType("datetime2");

                    b.Property<int>("user_id")
                        .HasColumnType("int");

                    b.Property<string>("value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("user_id");

                    b.ToTable("JWTs");
                });

            modelBuilder.Entity("TestWebAPI.Models.Nofication", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("property_id")
                        .HasColumnType("int");

                    b.Property<DateTime>("updatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("user_id")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("property_id");

                    b.HasIndex("user_id");

                    b.ToTable("Nofications");
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

                    b.Property<int>("category_id")
                        .HasColumnType("int");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("description")
                        .HasColumnType("bigint");

                    b.Property<int>("price")
                        .HasColumnType("int");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("updatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("category_id");

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

                    b.Property<string>("city")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("images")
                        .HasColumnType("bigint");

                    b.Property<int>("property_id")
                        .HasColumnType("int");

                    b.Property<string>("province")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("seller_id")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("property_id");

                    b.HasIndex("seller_id");

                    b.ToTable("PropertyHasDetail");
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

            modelBuilder.Entity("TestWebAPI.Models.Submission", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("buyer_id")
                        .HasColumnType("int");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("message")
                        .HasColumnType("bigint");

                    b.Property<int>("property_id")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("buyer_id");

                    b.HasIndex("property_id");

                    b.ToTable("Submissions");
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
                        .IsRequired()
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

                    b.Property<int>("user_id")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("user_id");

                    b.ToTable("User_Medias");
                });

            modelBuilder.Entity("TestWebAPI.Models.Wishlist", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("buyer_id")
                        .HasColumnType("int");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("property_id")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("buyer_id");

                    b.HasIndex("property_id");

                    b.ToTable("Wishlists");
                });

            modelBuilder.Entity("TestWebAPI.Models.Appointment", b =>
                {
                    b.HasOne("TestWebAPI.Models.User", "buyer")
                        .WithMany("Appointments")
                        .HasForeignKey("buyer_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestWebAPI.Models.Property", "property")
                        .WithMany("Appointments")
                        .HasForeignKey("property_id")
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
                        .WithMany("Contracts")
                        .HasForeignKey("appointment_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("appointment");
                });

            modelBuilder.Entity("TestWebAPI.Models.Evaluate", b =>
                {
                    b.HasOne("TestWebAPI.Models.User", "buyer")
                        .WithMany("Evaluates")
                        .HasForeignKey("buyer_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestWebAPI.Models.Property", "property")
                        .WithMany("Evaluates")
                        .HasForeignKey("property_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("buyer");

                    b.Navigation("property");
                });

            modelBuilder.Entity("TestWebAPI.Models.JWT", b =>
                {
                    b.HasOne("TestWebAPI.Models.User", "user")
                        .WithMany("JWTs")
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("TestWebAPI.Models.Nofication", b =>
                {
                    b.HasOne("TestWebAPI.Models.Property", "property")
                        .WithMany("Nofications")
                        .HasForeignKey("property_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestWebAPI.Models.User", "user")
                        .WithMany("Nofications")
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("property");

                    b.Navigation("user");
                });

            modelBuilder.Entity("TestWebAPI.Models.Property", b =>
                {
                    b.HasOne("TestWebAPI.Models.Category", "category")
                        .WithMany("Properties")
                        .HasForeignKey("category_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("category");
                });

            modelBuilder.Entity("TestWebAPI.Models.PropertyHasDetail", b =>
                {
                    b.HasOne("TestWebAPI.Models.Property", "property")
                        .WithMany("PropertyHasDetails")
                        .HasForeignKey("property_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestWebAPI.Models.User", "seller")
                        .WithMany("PropertyHasDetails")
                        .HasForeignKey("seller_id")
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

            modelBuilder.Entity("TestWebAPI.Models.Submission", b =>
                {
                    b.HasOne("TestWebAPI.Models.User", "buyer")
                        .WithMany("Submissions")
                        .HasForeignKey("buyer_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestWebAPI.Models.Property", "property")
                        .WithMany("Submissions")
                        .HasForeignKey("property_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("buyer");

                    b.Navigation("property");
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
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("TestWebAPI.Models.Wishlist", b =>
                {
                    b.HasOne("TestWebAPI.Models.User", "buyer")
                        .WithMany("Wishlists")
                        .HasForeignKey("buyer_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestWebAPI.Models.Property", "property")
                        .WithMany("Wishlists")
                        .HasForeignKey("property_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("buyer");

                    b.Navigation("property");
                });

            modelBuilder.Entity("TestWebAPI.Models.Appointment", b =>
                {
                    b.Navigation("Contracts");
                });

            modelBuilder.Entity("TestWebAPI.Models.Category", b =>
                {
                    b.Navigation("Properties");
                });

            modelBuilder.Entity("TestWebAPI.Models.Permission", b =>
                {
                    b.Navigation("Role_Permissions");
                });

            modelBuilder.Entity("TestWebAPI.Models.Property", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Contracts");

                    b.Navigation("Evaluates");

                    b.Navigation("Nofications");

                    b.Navigation("PropertyHasDetails");

                    b.Navigation("Submissions");

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

                    b.Navigation("Evaluates");

                    b.Navigation("JWTs");

                    b.Navigation("Nofications");

                    b.Navigation("PropertyHasDetails");

                    b.Navigation("Submissions");

                    b.Navigation("User_Medias");

                    b.Navigation("Wishlists");
                });
#pragma warning restore 612, 618
        }
    }
}
