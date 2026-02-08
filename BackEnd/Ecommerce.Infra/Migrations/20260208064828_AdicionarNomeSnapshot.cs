using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infra.Migrations
{
    public partial class AdicionarNomeSnapshot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NomeProdutoSnapshot",
                table: "ItensPedido",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomeProdutoSnapshot",
                table: "ItensPedido");
        }
    }
}
