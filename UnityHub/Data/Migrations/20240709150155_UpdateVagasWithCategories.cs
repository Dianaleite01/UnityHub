using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnityHub.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVagasWithCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5d9b217b-2b50-49e0-8392-afb4b671be8d", "AQAAAAIAAYagAAAAEMxHTcsd4ALiev85JYmaf7HqoP+7dFgI2lTNiRytXHihB/vBaDFaDBjujxEf9VOycw==", "95abf1ec-2e15-4841-9052-62a6b10e388d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ace91c23-50f7-4210-a10b-2477299a12f1", "AQAAAAIAAYagAAAAEEkkMul7o+e4N04IBknB/GrSViLBe5xmlu7qU0JyHRob5O2Iwcb2OXKCOGaiSif4zg==", "93e7e475-8248-48ae-8017-d79455e83d52" });
        }
    }
}
