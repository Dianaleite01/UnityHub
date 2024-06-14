using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UnityHub.Migrations
{
    /// <inheritdoc />
    public partial class RolesAdicionadas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NomeUtilizador",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "admin", null, "admin", "ADMIN" },
                    { "registado", null, "registado", "REGISTADO" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NomeUtilizador", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "admin", 0, "cf6baa45-932f-4f54-adee-7649b6caf7de", "admin@UnityHub.pt", true, false, null, "Administrador", "ADMIN@UnityHub.PT", "ADMIN@UNITYHUB.PT", "AQAAAAIAAYagAAAAEEzsL1CWRxFQQZmSDm3Tn5ia2K+TTT0lTdUXnnnlrugjVpoWZMi4eLG2R51OR1GMuA==", null, false, "200ba98e-6d32-49b2-9e9e-d2e967a99189", false, "admin@UnityHub.pt" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "admin", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "registado");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "admin", "admin" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin");

            migrationBuilder.DropColumn(
                name: "NomeUtilizador",
                table: "AspNetUsers");
        }
    }
}
