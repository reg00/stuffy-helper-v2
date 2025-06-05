using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StuffyHelper.Data.Migrations
{
    public partial class AddPurchaseType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PurchaseTypeId",
                table: "purchase",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "purchase-types",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchase-types", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_purchase_PurchaseTypeId",
                table: "purchase",
                column: "PurchaseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_purchase-types_Name",
                table: "purchase-types",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_purchase_purchase-types_PurchaseTypeId",
                table: "purchase",
                column: "PurchaseTypeId",
                principalTable: "purchase-types",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_purchase_purchase-types_PurchaseTypeId",
                table: "purchase");

            migrationBuilder.DropTable(
                name: "purchase-types");

            migrationBuilder.DropIndex(
                name: "IX_purchase_PurchaseTypeId",
                table: "purchase");

            migrationBuilder.DropColumn(
                name: "PurchaseTypeId",
                table: "purchase");
        }
    }
}
