using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnityHub.Migrations
{
    /// <inheritdoc />
    public partial class FogografiaVagas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Fotografia",
                table: "Vagas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1b132e24-72e3-4c31-bd19-0a8a0eb7e64f", "AQAAAAIAAYagAAAAECXqi688EexC7p8PdzotpTaxKS2uSpbB94Tfsry9ZPohRJuCKr84tZX5ebPXURdeUg==", "7063dc8a-2a11-4c2c-b5b8-06f027ead4a7" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fotografia",
                table: "Vagas");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5d9b217b-2b50-49e0-8392-afb4b671be8d", "AQAAAAIAAYagAAAAEMxHTcsd4ALiev85JYmaf7HqoP+7dFgI2lTNiRytXHihB/vBaDFaDBjujxEf9VOycw==", "95abf1ec-2e15-4841-9052-62a6b10e388d" });
        }
    }
}
