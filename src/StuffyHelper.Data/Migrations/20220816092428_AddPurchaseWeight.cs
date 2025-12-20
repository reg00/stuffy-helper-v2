using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StuffyHelper.Data.Migrations
{
    public partial class AddPurchaseWeight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "purchase",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight",
                table: "purchase");
        }
    }
}
