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
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    avatar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId1 = table.Column<int>(type: "int", nullable: false),
                    userId2 = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.id);
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
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<int>(type: "int", nullable: false),
                    avatar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    categoryId = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.id);
                    table.ForeignKey(
                        name: "FK_Properties_Categories_categoryId",
                        column: x => x.categoryId,
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
                    buyerId = table.Column<int>(type: "int", nullable: false),
                    propertyId = table.Column<int>(type: "int", nullable: false),
                    appointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    backupDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.id);
                    table.ForeignKey(
                        name: "FK_Appointments_Properties_propertyId",
                        column: x => x.propertyId,
                        principalTable: "Properties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Users_buyerId",
                        column: x => x.buyerId,
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
                    buyerId = table.Column<int>(type: "int", nullable: false),
                    propertyId = table.Column<int>(type: "int", nullable: false),
                    star = table.Column<int>(type: "int", nullable: false),
                    review = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evaluates", x => x.id);
                    table.ForeignKey(
                        name: "FK_Evaluates_Properties_propertyId",
                        column: x => x.propertyId,
                        principalTable: "Properties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Evaluates_Users_buyerId",
                        column: x => x.buyerId,
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
                    issuedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    expiredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JWTs", x => x.id);
                    table.ForeignKey(
                        name: "FK_JWTs_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false),
                    conversationId = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.id);
                    table.ForeignKey(
                        name: "FK_Messages_Conversations_conversationId",
                        column: x => x.conversationId,
                        principalTable: "Conversations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    propertyId = table.Column<int>(type: "int", nullable: false),
                    conversationId = table.Column<int>(type: "int", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_Notifications_Conversations_conversationId",
                        column: x => x.conversationId,
                        principalTable: "Conversations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notifications_Properties_propertyId",
                        column: x => x.propertyId,
                        principalTable: "Properties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyHasDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    images = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bedroom = table.Column<int>(type: "int", nullable: false),
                    bathroom = table.Column<int>(type: "int", nullable: false),
                    yearBuild = table.Column<int>(type: "int", nullable: false),
                    size = table.Column<int>(type: "int", nullable: false),
                    sellerId = table.Column<int>(type: "int", nullable: false),
                    propertyId = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyHasDetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_PropertyHasDetails_Properties_propertyId",
                        column: x => x.propertyId,
                        principalTable: "Properties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropertyHasDetails_Users_sellerId",
                        column: x => x.sellerId,
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
                    userId = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Medias", x => x.id);
                    table.ForeignKey(
                        name: "FK_User_Medias_Users_userId",
                        column: x => x.userId,
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
                    buyerId = table.Column<int>(type: "int", nullable: false),
                    propertyId = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishlists", x => x.id);
                    table.ForeignKey(
                        name: "FK_Wishlists_Properties_propertyId",
                        column: x => x.propertyId,
                        principalTable: "Properties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wishlists_Users_buyerId",
                        column: x => x.buyerId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    appointmentId = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.id);
                    table.ForeignKey(
                        name: "FK_Offers_Appointments_appointmentId",
                        column: x => x.appointmentId,
                        principalTable: "Appointments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    signatureBuyer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    signatureSeller = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    offerId = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Propertyid = table.Column<int>(type: "int", nullable: true),
                    Userid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.id);
                    table.ForeignKey(
                        name: "FK_Contracts_Offers_offerId",
                        column: x => x.offerId,
                        principalTable: "Offers",
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
                name: "IX_Appointments_buyerId",
                table: "Appointments",
                column: "buyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_propertyId",
                table: "Appointments",
                column: "propertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_offerId",
                table: "Contracts",
                column: "offerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_Propertyid",
                table: "Contracts",
                column: "Propertyid");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_Userid",
                table: "Contracts",
                column: "Userid");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluates_buyerId",
                table: "Evaluates",
                column: "buyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluates_propertyId",
                table: "Evaluates",
                column: "propertyId");

            migrationBuilder.CreateIndex(
                name: "IX_JWTs_userId",
                table: "JWTs",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_conversationId",
                table: "Messages",
                column: "conversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_userId",
                table: "Messages",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_conversationId",
                table: "Notifications",
                column: "conversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_propertyId",
                table: "Notifications",
                column: "propertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_userId",
                table: "Notifications",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_appointmentId",
                table: "Offers",
                column: "appointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_categoryId",
                table: "Properties",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyHasDetails_propertyId",
                table: "PropertyHasDetails",
                column: "propertyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropertyHasDetails_sellerId",
                table: "PropertyHasDetails",
                column: "sellerId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_codePermission",
                table: "RolePermissions",
                column: "codePermission");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_codeRole",
                table: "RolePermissions",
                column: "codeRole");

            migrationBuilder.CreateIndex(
                name: "IX_User_Medias_userId",
                table: "User_Medias",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_roleCode",
                table: "Users",
                column: "roleCode");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_buyerId",
                table: "Wishlists",
                column: "buyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_propertyId",
                table: "Wishlists",
                column: "propertyId");
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
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "PropertyHasDetails");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "User_Medias");

            migrationBuilder.DropTable(
                name: "Wishlists");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Appointments");

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
