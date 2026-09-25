using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoFinal.API.Migrations
{
    /// <inheritdoc />
    public partial class AtualizacaoEstrutura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "Interacoes");

            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "Chamados");

            migrationBuilder.AddColumn<string>(
                name: "Autor",
                table: "Interacoes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DataRegistro",
                table: "Interacoes",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<string>(
                name: "Titulo",
                table: "Chamados",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DataAbertura",
                table: "Chamados",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DataFechamento",
                table: "Chamados",
                type: "datetimeoffset",
                nullable: true);

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

            migrationBuilder.AddCheckConstraint(
                name: "CK_Chamados_Prioridade",
                table: "Chamados",
                sql: "[Prioridade] IN (1, 2, 3)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Chamados_Status",
                table: "Chamados",
                sql: "[Status] IN (1, 2, 3)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Chamados_Prioridade",
                table: "Chamados");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Chamados_Status",
                table: "Chamados");

            migrationBuilder.DropColumn(
                name: "Autor",
                table: "Interacoes");

            migrationBuilder.DropColumn(
                name: "DataRegistro",
                table: "Interacoes");

            migrationBuilder.DropColumn(
                name: "DataAbertura",
                table: "Chamados");

            migrationBuilder.DropColumn(
                name: "DataFechamento",
                table: "Chamados");

            migrationBuilder.DropColumn(
                name: "SolicitanteNome",
                table: "Chamados");

            migrationBuilder.DropColumn(
                name: "Solucao",
                table: "Chamados");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "Interacoes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Titulo",
                table: "Chamados",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "Chamados",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
