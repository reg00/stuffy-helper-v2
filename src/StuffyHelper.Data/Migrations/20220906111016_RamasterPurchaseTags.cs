using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StuffyHelper.Data.Migrations
{
    public partial class RamasterPurchaseTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "purchase-tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchase-tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseEntryPurchaseTagEntry",
                columns: table => new
                {
                    PurchaseTagsId = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchasesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseEntryPurchaseTagEntry", x => new { x.PurchaseTagsId, x.PurchasesId });
                    table.ForeignKey(
                        name: "FK_PurchaseEntryPurchaseTagEntry_purchase_PurchasesId",
                        column: x => x.PurchasesId,
                        principalTable: "purchase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseEntryPurchaseTagEntry_purchase-tags_PurchaseTagsId",
                        column: x => x.PurchaseTagsId,
                        principalTable: "purchase-tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_purchase-tags_Name",
                table: "purchase-tags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseEntryPurchaseTagEntry_PurchasesId",
                table: "PurchaseEntryPurchaseTagEntry",
                column: "PurchasesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseEntryPurchaseTagEntry");

            migrationBuilder.DropTable(
                name: "purchase-tags");

            migrationBuilder.AddColumn<Guid>(
                name: "PurchaseTypeId",
                table: "purchase",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "purchase-types",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
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
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
