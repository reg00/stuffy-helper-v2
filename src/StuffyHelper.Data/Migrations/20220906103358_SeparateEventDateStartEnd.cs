using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StuffyHelper.Data.Migrations
{
    public partial class SeparateEventDateStartEnd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EventDate",
                table: "event",
                newName: "EventDateStart");

            migrationBuilder.RenameIndex(
                name: "IX_event_Name_EventDate",
                table: "event",
                newName: "IX_event_Name_EventDateStart");

            migrationBuilder.AddColumn<DateTime>(
                name: "EventDateEnd",
                table: "event",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventDateEnd",
                table: "event");

            migrationBuilder.RenameColumn(
                name: "EventDateStart",
                table: "event",
                newName: "EventDate");

            migrationBuilder.RenameIndex(
                name: "IX_event_Name_EventDateStart",
                table: "event",
                newName: "IX_event_Name_EventDate");
        }
    }
}
