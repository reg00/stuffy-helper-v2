using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StuffyHelper.Authorization.EntityFrameworkCore1.Migrations
{
    public partial class UpdateStuffyUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NickName",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
