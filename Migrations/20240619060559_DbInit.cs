using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class DbInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<long>(type: "bigint", nullable: false),
                    avatar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.id);
                    table.UniqueConstraint("AK_Permissions_code", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    createAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.id);
                    table.UniqueConstraint("AK_Roles_code", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<long>(type: "bigint", nullable: false),
                    price = table.Column<int>(type: "int", nullable: false),
                    avatar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    category_id = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.id);
                    table.ForeignKey(
                        name: "FK_Properties_Categories_category_id",
                        column: x => x.category_id,
                        principalTable: "Categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codeRole = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    codePermission = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.id);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_codePermission",
                        column: x => x.codePermission,
                        principalTable: "Permissions",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_codeRole",
                        column: x => x.codeRole,
                        principalTable: "Roles",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    phone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    first_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    roleCode = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    passwordChangeAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    passwordResetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    passwordResetExpires = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_roleCode",
                        column: x => x.roleCode,
                        principalTable: "Roles",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    buyer_id = table.Column<int>(type: "int", nullable: false),
                    property_id = table.Column<int>(type: "int", nullable: false),
                    appointment_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    secondary_day = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.id);
                    table.ForeignKey(
                        name: "FK_Appointments_Properties_property_id",
                        column: x => x.property_id,
                        principalTable: "Properties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Users_buyer_id",
                        column: x => x.buyer_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Evaluates",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    buyer_id = table.Column<int>(type: "int", nullable: false),
                    property_id = table.Column<int>(type: "int", nullable: false),
                    star = table.Column<int>(type: "int", nullable: false),
                    review = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evaluates", x => x.id);
                    table.ForeignKey(
                        name: "FK_Evaluates_Properties_property_id",
                        column: x => x.property_id,
                        principalTable: "Properties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Evaluates_Users_buyer_id",
                        column: x => x.buyer_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JWTs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    issued_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    expired_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JWTs", x => x.id);
                    table.ForeignKey(
                        name: "FK_JWTs_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Nofications",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    property_id = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nofications", x => x.id);
                    table.ForeignKey(
                        name: "FK_Nofications_Properties_property_id",
                        column: x => x.property_id,
                        principalTable: "Properties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Nofications_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyHasDetail",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    images = table.Column<long>(type: "bigint", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    seller_id = table.Column<int>(type: "int", nullable: false),
                    property_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyHasDetail", x => x.id);
                    table.ForeignKey(
                        name: "FK_PropertyHasDetail_Properties_property_id",
                        column: x => x.property_id,
                        principalTable: "Properties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropertyHasDetail_Users_seller_id",
                        column: x => x.seller_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    buyer_id = table.Column<int>(type: "int", nullable: false),
                    property_id = table.Column<int>(type: "int", nullable: false),
                    message = table.Column<long>(type: "bigint", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.id);
                    table.ForeignKey(
                        name: "FK_Submissions_Properties_property_id",
                        column: x => x.property_id,
                        principalTable: "Properties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Submissions_Users_buyer_id",
                        column: x => x.buyer_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User_Medias",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    provider = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Medias", x => x.id);
                    table.ForeignKey(
                        name: "FK_User_Medias_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wishlists",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    buyer_id = table.Column<int>(type: "int", nullable: false),
                    property_id = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishlists", x => x.id);
                    table.ForeignKey(
                        name: "FK_Wishlists_Properties_property_id",
                        column: x => x.property_id,
                        principalTable: "Properties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wishlists_Users_buyer_id",
                        column: x => x.buyer_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    signature_buyer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    signature_seller = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    appointment_id = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Propertyid = table.Column<int>(type: "int", nullable: true),
                    Userid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.id);
                    table.ForeignKey(
                        name: "FK_Contracts_Appointments_appointment_id",
                        column: x => x.appointment_id,
                        principalTable: "Appointments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contracts_Properties_Propertyid",
                        column: x => x.Propertyid,
                        principalTable: "Properties",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Contracts_Users_Userid",
                        column: x => x.Userid,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_buyer_id",
                table: "Appointments",
                column: "buyer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_property_id",
                table: "Appointments",
                column: "property_id");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_appointment_id",
                table: "Contracts",
                column: "appointment_id");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_Propertyid",
                table: "Contracts",
                column: "Propertyid");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_Userid",
                table: "Contracts",
                column: "Userid");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluates_buyer_id",
                table: "Evaluates",
                column: "buyer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluates_property_id",
                table: "Evaluates",
                column: "property_id");

            migrationBuilder.CreateIndex(
                name: "IX_JWTs_user_id",
                table: "JWTs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Nofications_property_id",
                table: "Nofications",
                column: "property_id");

            migrationBuilder.CreateIndex(
                name: "IX_Nofications_user_id",
                table: "Nofications",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_category_id",
                table: "Properties",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyHasDetail_property_id",
                table: "PropertyHasDetail",
                column: "property_id");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyHasDetail_seller_id",
                table: "PropertyHasDetail",
                column: "seller_id");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_codePermission",
                table: "RolePermissions",
                column: "codePermission");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_codeRole",
                table: "RolePermissions",
                column: "codeRole");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_buyer_id",
                table: "Submissions",
                column: "buyer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_property_id",
                table: "Submissions",
                column: "property_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Medias_user_id",
                table: "User_Medias",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_roleCode",
                table: "Users",
                column: "roleCode");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_buyer_id",
                table: "Wishlists",
                column: "buyer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_property_id",
                table: "Wishlists",
                column: "property_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "Evaluates");

            migrationBuilder.DropTable(
                name: "JWTs");

            migrationBuilder.DropTable(
                name: "Nofications");

            migrationBuilder.DropTable(
                name: "PropertyHasDetail");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.DropTable(
                name: "User_Medias");

            migrationBuilder.DropTable(
                name: "Wishlists");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
