using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StuffyHelper.Data.Migrations
{
    /// <inheritdoc />
    public partial class removeunusedtables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_purchase_unit-types_UnitTypeId",
                table: "purchase");

            migrationBuilder.DropTable(
                name: "PurchaseEntryPurchaseTagEntry");

            migrationBuilder.DropTable(
                name: "unit-types");

            migrationBuilder.DropTable(
                name: "purchase-tags");

            migrationBuilder.DropIndex(
                name: "IX_purchase_UnitTypeId",
                table: "purchase");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "purchase");

            migrationBuilder.DropColumn(
                name: "IsPartial",
                table: "purchase");

            migrationBuilder.DropColumn(
                name: "UnitTypeId",
                table: "purchase");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Amount",
                table: "purchase",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsPartial",
                table: "purchase",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UnitTypeId",
                table: "purchase",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "purchase-tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchase-tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "unit-types",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_unit-types", x => x.Id);
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
                        name: "FK_PurchaseEntryPurchaseTagEntry_purchase-tags_PurchaseTagsId",
                        column: x => x.PurchaseTagsId,
                        principalTable: "purchase-tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseEntryPurchaseTagEntry_purchase_PurchasesId",
                        column: x => x.PurchasesId,
                        principalTable: "purchase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "unit-types",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("320053b6-110a-4358-9289-21e64d718b60"), true, "Мл." },
                    { new Guid("32939f8e-3818-4753-8d1b-4ba8ab7783f9"), true, "Кг." },
                    { new Guid("6700cac9-f36e-4697-a6fe-27fdbaebd267"), true, "Л." },
                    { new Guid("7142d1aa-53b1-416c-80b8-18b5d3ba33ab"), true, "Шт." },
                    { new Guid("eda2a0fe-539c-471d-9941-e0ce8982e923"), true, "Уп." },
                    { new Guid("f73043eb-9e20-4934-845a-7722557f164e"), true, "Гр." }
                });

            migrationBuilder.CreateIndex(
                name: "IX_purchase_UnitTypeId",
                table: "purchase",
                column: "UnitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_purchase-tags_Name",
                table: "purchase-tags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseEntryPurchaseTagEntry_PurchasesId",
                table: "PurchaseEntryPurchaseTagEntry",
                column: "PurchasesId");

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
    }
}
