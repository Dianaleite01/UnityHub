using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnityHub.Migrations
{
    /// <inheritdoc />
    public partial class ModeloDadosAtualizados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidaturas_Voluntariados_VoluntariadoFK",
                table: "Candidaturas");

            migrationBuilder.DropTable(
                name: "Voluntariados");

            migrationBuilder.RenameColumn(
                name: "VoluntariadoFK",
                table: "Candidaturas",
                newName: "VagaFK");

            migrationBuilder.RenameIndex(
                name: "IX_Candidaturas_VoluntariadoFK",
                table: "Candidaturas",
                newName: "IX_Candidaturas_VagaFK");

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vagas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PeriodoVoluntariado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Local = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UtilizadoresId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vagas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vagas_Utilizadores_UtilizadoresId",
                        column: x => x.UtilizadoresId,
                        principalTable: "Utilizadores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VagaCategoria",
                columns: table => new
                {
                    CategoriasId = table.Column<int>(type: "int", nullable: false),
                    VagasCategoriasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VagaCategoria", x => new { x.CategoriasId, x.VagasCategoriasId });
                    table.ForeignKey(
                        name: "FK_VagaCategoria_Categorias_CategoriasId",
                        column: x => x.CategoriasId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VagaCategoria_Vagas_VagasCategoriasId",
                        column: x => x.VagasCategoriasId,
                        principalTable: "Vagas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "024b333d-d395-4641-acac-cdc51e053362", "AQAAAAIAAYagAAAAEDVVq3y/RPbERroy+iOlzFGyBF/iEegODY0XioAiE3jHub2eULAHiqXKwWTrlnYi0A==", "c37a33ca-4278-4745-9db7-3ce58e8c22d8" });

            migrationBuilder.CreateIndex(
                name: "IX_VagaCategoria_VagasCategoriasId",
                table: "VagaCategoria",
                column: "VagasCategoriasId");

            migrationBuilder.CreateIndex(
                name: "IX_Vagas_UtilizadoresId",
                table: "Vagas",
                column: "UtilizadoresId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidaturas_Vagas_VagaFK",
                table: "Candidaturas",
                column: "VagaFK",
                principalTable: "Vagas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidaturas_Vagas_VagaFK",
                table: "Candidaturas");

            migrationBuilder.DropTable(
                name: "VagaCategoria");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Vagas");

            migrationBuilder.RenameColumn(
                name: "VagaFK",
                table: "Candidaturas",
                newName: "VoluntariadoFK");

            migrationBuilder.RenameIndex(
                name: "IX_Candidaturas_VagaFK",
                table: "Candidaturas",
                newName: "IX_Candidaturas_VoluntariadoFK");

            migrationBuilder.CreateTable(
                name: "Voluntariados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Local = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PeriodoVoluntariado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UtilizadoresId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voluntariados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Voluntariados_Utilizadores_UtilizadoresId",
                        column: x => x.UtilizadoresId,
                        principalTable: "Utilizadores",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cf6baa45-932f-4f54-adee-7649b6caf7de", "AQAAAAIAAYagAAAAEEzsL1CWRxFQQZmSDm3Tn5ia2K+TTT0lTdUXnnnlrugjVpoWZMi4eLG2R51OR1GMuA==", "200ba98e-6d32-49b2-9e9e-d2e967a99189" });

            migrationBuilder.CreateIndex(
                name: "IX_Voluntariados_UtilizadoresId",
                table: "Voluntariados",
                column: "UtilizadoresId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidaturas_Voluntariados_VoluntariadoFK",
                table: "Candidaturas",
                column: "VoluntariadoFK",
                principalTable: "Voluntariados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
