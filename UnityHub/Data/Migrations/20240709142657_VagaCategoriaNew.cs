using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnityHub.Migrations
{
    /// <inheritdoc />
    public partial class VagaCategoriaNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ace91c23-50f7-4210-a10b-2477299a12f1", "AQAAAAIAAYagAAAAEEkkMul7o+e4N04IBknB/GrSViLBe5xmlu7qU0JyHRob5O2Iwcb2OXKCOGaiSif4zg==", "93e7e475-8248-48ae-8017-d79455e83d52" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fcca16a5-e91b-4b06-b1bf-88702f69dfed", "AQAAAAIAAYagAAAAEJGz9OWuLpgQ8DkECQlaTfgnPHLHkFmEbysJ6hyaJJVkd1ISHWrys3v8RoB4grn5Mg==", "84a852a8-3f36-43e3-9a11-5c3af630292a" });
        }
    }
}
