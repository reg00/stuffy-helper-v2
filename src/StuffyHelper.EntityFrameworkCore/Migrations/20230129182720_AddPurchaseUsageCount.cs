using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StuffyHelper.EntityFrameworkCore.Migrations
{
    public partial class AddPurchaseUsageCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "purchase-usage",
                type: "integer",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "purchase-usage");
        }
    }
}
