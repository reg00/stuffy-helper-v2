using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StuffyHelper.Data.Migrations
{
    public partial class AddCheckout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CheckoutId",
                table: "purchase-usage",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CheckoutId",
                table: "debts",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "checkouts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checkouts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_checkouts_event_EventId",
                        column: x => x.EventId,
                        principalTable: "event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_purchase-usage_CheckoutId",
                table: "purchase-usage",
                column: "CheckoutId");

            migrationBuilder.CreateIndex(
                name: "IX_debts_CheckoutId",
                table: "debts",
                column: "CheckoutId");

            migrationBuilder.CreateIndex(
                name: "IX_checkouts_EventId",
                table: "checkouts",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_debts_checkouts_CheckoutId",
                table: "debts",
                column: "CheckoutId",
                principalTable: "checkouts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_purchase-usage_checkouts_CheckoutId",
                table: "purchase-usage",
                column: "CheckoutId",
                principalTable: "checkouts",
                principalColumn: "Id");

            migrationBuilder.Sql("CREATE EXTENSION pgcrypto;");
            migrationBuilder.Sql("INSERT INTO checkouts(\"Id\", \"EventId\", \"CreatedDate\") SELECT gen_random_uuid(), d.\"EventId\", timezone('utc', now()) FROM debts d GROUP BY d.\"EventId\";");
            migrationBuilder.Sql("UPDATE debts SET \"CheckoutId\" = c.\"Id\" FROM checkouts c WHERE debts.\"EventId\" = c.\"EventId\";");
            migrationBuilder.Sql("UPDATE \"purchase-usage\" SET \"CheckoutId\" = c.\"Id\" FROM checkouts c INNER JOIN purchase pc ON pc.\"EventId\" = c.\"EventId\" AND pc.\"IsComplete\" = true WHERE \"purchase-usage\".\"PurchaseId\" = pc.\"Id\";");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_debts_checkouts_CheckoutId",
                table: "debts");

            migrationBuilder.DropForeignKey(
                name: "FK_purchase-usage_checkouts_CheckoutId",
                table: "purchase-usage");

            migrationBuilder.DropTable(
                name: "checkouts");

            migrationBuilder.DropIndex(
                name: "IX_purchase-usage_CheckoutId",
                table: "purchase-usage");

            migrationBuilder.DropIndex(
                name: "IX_debts_CheckoutId",
                table: "debts");

            migrationBuilder.DropColumn(
                name: "CheckoutId",
                table: "purchase-usage");

            migrationBuilder.DropColumn(
                name: "CheckoutId",
                table: "debts");
        }
    }
}
