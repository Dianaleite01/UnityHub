using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnityHub.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomUserProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidaturas_Utilizadores_UtilizadorFK",
                table: "Candidaturas");

            migrationBuilder.DropForeignKey(
                name: "FK_Vagas_Utilizadores_UtilizadoresId",
                table: "Vagas");

            migrationBuilder.DropTable(
                name: "Utilizadores");

            migrationBuilder.DropColumn(
                name: "NomeUtilizador",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UtilizadoresId",
                table: "Vagas",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UtilizadorFK",
                table: "Candidaturas",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "DataNascimento",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Pais",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telemovel",
                table: "AspNetUsers",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "Cidade", "ConcurrencyStamp", "DataNascimento", "Nome", "Pais", "PasswordHash", "SecurityStamp", "Telemovel" },
                values: new object[] { "CidadeAdmin", "b0d6d45a-9e18-4e03-ac18-b89f93f38fc4", new DateOnly(1980, 1, 1), "Administrador", "PaisAdmin", "AQAAAAIAAYagAAAAEH9bQrqboUI1t4VeajtRvXRzXkf+g90XF1h45UIn6YKP3MdLVVM4hQElb/p9hL3VaQ==", "6ca756f6-469a-4041-b55e-30de03037274", "912345678" });

            migrationBuilder.AddForeignKey(
                name: "FK_Candidaturas_AspNetUsers_UtilizadorFK",
                table: "Candidaturas",
                column: "UtilizadorFK",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vagas_AspNetUsers_UtilizadoresId",
                table: "Vagas",
                column: "UtilizadoresId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidaturas_AspNetUsers_UtilizadorFK",
                table: "Candidaturas");

            migrationBuilder.DropForeignKey(
                name: "FK_Vagas_AspNetUsers_UtilizadoresId",
                table: "Vagas");

            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DataNascimento",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Pais",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Telemovel",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "UtilizadoresId",
                table: "Vagas",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UtilizadorFK",
                table: "Candidaturas",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "NomeUtilizador",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Utilizadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cidade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataNascimento = table.Column<DateOnly>(type: "date", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Pais = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Telemovel = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizadores", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "NomeUtilizador", "PasswordHash", "SecurityStamp" },
                values: new object[] { "024b333d-d395-4641-acac-cdc51e053362", "Administrador", "AQAAAAIAAYagAAAAEDVVq3y/RPbERroy+iOlzFGyBF/iEegODY0XioAiE3jHub2eULAHiqXKwWTrlnYi0A==", "c37a33ca-4278-4745-9db7-3ce58e8c22d8" });

            migrationBuilder.AddForeignKey(
                name: "FK_Candidaturas_Utilizadores_UtilizadorFK",
                table: "Candidaturas",
                column: "UtilizadorFK",
                principalTable: "Utilizadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vagas_Utilizadores_UtilizadoresId",
                table: "Vagas",
                column: "UtilizadoresId",
                principalTable: "Utilizadores",
                principalColumn: "Id");
        }
    }
}
