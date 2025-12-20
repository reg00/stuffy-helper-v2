using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StuffyHelper.Data.Migrations
{
    public partial class AddUnitType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UnitTypeId",
                table: "purchase",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "unit-types",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_unit-types", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_purchase_UnitTypeId",
                table: "purchase",
                column: "UnitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_unit-types_Name",
                table: "unit-types",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_purchase_unit-types_UnitTypeId",
                table: "purchase",
                column: "UnitTypeId",
                principalTable: "unit-types",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_purchase_unit-types_UnitTypeId",
                table: "purchase");

            migrationBuilder.DropTable(
                name: "unit-types");

            migrationBuilder.DropIndex(
                name: "IX_purchase_UnitTypeId",
                table: "purchase");

            migrationBuilder.DropColumn(
                name: "UnitTypeId",
                table: "purchase");
        }
    }
}
