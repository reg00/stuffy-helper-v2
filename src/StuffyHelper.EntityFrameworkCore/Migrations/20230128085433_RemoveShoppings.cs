using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StuffyHelper.EntityFrameworkCore.Migrations
{
    public partial class RemoveShoppings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_purchase_shopping_ShoppingId",
                table: "purchase");

            migrationBuilder.DropTable(
                name: "shopping");

            migrationBuilder.RenameColumn(
                name: "ShoppingId",
                table: "purchase",
                newName: "ParticipantId");

            migrationBuilder.RenameIndex(
                name: "IX_purchase_ShoppingId",
                table: "purchase",
                newName: "IX_purchase_ParticipantId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "purchase",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "purchase",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_purchase_EventId",
                table: "purchase",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_purchase_event_EventId",
                table: "purchase",
                column: "EventId",
                principalTable: "event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_purchase_participant_ParticipantId",
                table: "purchase",
                column: "ParticipantId",
                principalTable: "participant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_purchase_event_EventId",
                table: "purchase");

            migrationBuilder.DropForeignKey(
                name: "FK_purchase_participant_ParticipantId",
                table: "purchase");

            migrationBuilder.DropIndex(
                name: "IX_purchase_EventId",
                table: "purchase");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "purchase");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "purchase");

            migrationBuilder.RenameColumn(
                name: "ParticipantId",
                table: "purchase",
                newName: "ShoppingId");

            migrationBuilder.RenameIndex(
                name: "IX_purchase_ParticipantId",
                table: "purchase",
                newName: "IX_purchase_ShoppingId");

            migrationBuilder.CreateTable(
                name: "shopping",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Check = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ShoppingDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shopping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shopping_event_EventId",
                        column: x => x.EventId,
                        principalTable: "event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_shopping_participant_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "participant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_shopping_EventId",
                table: "shopping",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_shopping_ParticipantId",
                table: "shopping",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_shopping_ShoppingDate",
                table: "shopping",
                column: "ShoppingDate");

            migrationBuilder.AddForeignKey(
                name: "FK_purchase_shopping_ShoppingId",
                table: "purchase",
                column: "ShoppingId",
                principalTable: "shopping",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
