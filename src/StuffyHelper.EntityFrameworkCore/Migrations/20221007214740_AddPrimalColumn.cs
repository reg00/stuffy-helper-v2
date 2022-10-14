using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StuffyHelper.EntityFrameworkCore.Migrations
{
    public partial class AddPrimalColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrimal",
                table: "media",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrimal",
                table: "media");
        }
    }
}
