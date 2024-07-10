using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnityHub.Migrations
{
    /// <inheritdoc />
    public partial class VagaCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VagaCategoria_Categorias_CategoriasId",
                table: "VagaCategoria");

            migrationBuilder.DropForeignKey(
                name: "FK_VagaCategoria_Vagas_VagasCategoriasId",
                table: "VagaCategoria");

            migrationBuilder.RenameColumn(
                name: "VagasCategoriasId",
                table: "VagaCategoria",
                newName: "CategoriaId");

            migrationBuilder.RenameColumn(
                name: "CategoriasId",
                table: "VagaCategoria",
                newName: "VagaId");

            migrationBuilder.RenameIndex(
                name: "IX_VagaCategoria_VagasCategoriasId",
                table: "VagaCategoria",
                newName: "IX_VagaCategoria_CategoriaId");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fcca16a5-e91b-4b06-b1bf-88702f69dfed", "AQAAAAIAAYagAAAAEJGz9OWuLpgQ8DkECQlaTfgnPHLHkFmEbysJ6hyaJJVkd1ISHWrys3v8RoB4grn5Mg==", "84a852a8-3f36-43e3-9a11-5c3af630292a" });

            migrationBuilder.AddForeignKey(
                name: "FK_VagaCategoria_Categorias_CategoriaId",
                table: "VagaCategoria",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VagaCategoria_Vagas_VagaId",
                table: "VagaCategoria",
                column: "VagaId",
                principalTable: "Vagas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VagaCategoria_Categorias_CategoriaId",
                table: "VagaCategoria");

            migrationBuilder.DropForeignKey(
                name: "FK_VagaCategoria_Vagas_VagaId",
                table: "VagaCategoria");

            migrationBuilder.RenameColumn(
                name: "CategoriaId",
                table: "VagaCategoria",
                newName: "VagasCategoriasId");

            migrationBuilder.RenameColumn(
                name: "VagaId",
                table: "VagaCategoria",
                newName: "CategoriasId");

            migrationBuilder.RenameIndex(
                name: "IX_VagaCategoria_CategoriaId",
                table: "VagaCategoria",
                newName: "IX_VagaCategoria_VagasCategoriasId");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b0d6d45a-9e18-4e03-ac18-b89f93f38fc4", "AQAAAAIAAYagAAAAEH9bQrqboUI1t4VeajtRvXRzXkf+g90XF1h45UIn6YKP3MdLVVM4hQElb/p9hL3VaQ==", "6ca756f6-469a-4041-b55e-30de03037274" });

            migrationBuilder.AddForeignKey(
                name: "FK_VagaCategoria_Categorias_CategoriasId",
                table: "VagaCategoria",
                column: "CategoriasId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VagaCategoria_Vagas_VagasCategoriasId",
                table: "VagaCategoria",
                column: "VagasCategoriasId",
                principalTable: "Vagas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
