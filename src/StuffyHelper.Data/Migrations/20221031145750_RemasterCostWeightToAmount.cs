using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StuffyHelper.Data.Migrations
{
    public partial class RemasterCostWeightToAmount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "purchase");

            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "purchase",
                newName: "Amount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "purchase",
                newName: "Weight");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "purchase",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
