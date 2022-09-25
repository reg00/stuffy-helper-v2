using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StuffyHelper.EntityFrameworkCore.Migrations
{
    public partial class AddPurchaseTypeRequiring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_purchase_purchase-types_PurchaseTypeId",
                table: "purchase");

            migrationBuilder.AlterColumn<Guid>(
                name: "PurchaseTypeId",
                table: "purchase",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_purchase_purchase-types_PurchaseTypeId",
                table: "purchase",
                column: "PurchaseTypeId",
                principalTable: "purchase-types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_purchase_purchase-types_PurchaseTypeId",
                table: "purchase");

            migrationBuilder.AlterColumn<Guid>(
                name: "PurchaseTypeId",
                table: "purchase",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_purchase_purchase-types_PurchaseTypeId",
                table: "purchase",
                column: "PurchaseTypeId",
                principalTable: "purchase-types",
                principalColumn: "Id");
        }
    }
}
