using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoFinal.API.Migrations
{
    /// <inheritdoc />
    public partial class CreateInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Autor",
                table: "Interacoes");

            migrationBuilder.DropColumn(
                name: "SolicitanteNome",
                table: "Chamados");

            migrationBuilder.DropColumn(
                name: "Solucao",
                table: "Chamados");

            migrationBuilder.RenameColumn(
                name: "DataRegistro",
                table: "Interacoes",
                newName: "DataCriacao");

            migrationBuilder.RenameColumn(
                name: "DataFechamento",
                table: "Chamados",
                newName: "DataAtualizacao");

            migrationBuilder.RenameColumn(
                name: "DataAbertura",
                table: "Chamados",
                newName: "DataCriacao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataCriacao",
                table: "Interacoes",
                newName: "DataRegistro");

            migrationBuilder.RenameColumn(
                name: "DataCriacao",
                table: "Chamados",
                newName: "DataAbertura");

            migrationBuilder.RenameColumn(
                name: "DataAtualizacao",
                table: "Chamados",
                newName: "DataFechamento");

            migrationBuilder.AddColumn<string>(
                name: "Autor",
                table: "Interacoes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SolicitanteNome",
                table: "Chamados",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Solucao",
                table: "Chamados",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
