using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StuffyHelper.Data.Migrations
{
    public partial class FixMediaStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_media_EventId_MediaUid",
                table: "media");

            migrationBuilder.RenameColumn(
                name: "MediaUid",
                table: "media",
                newName: "FileName");

            migrationBuilder.CreateIndex(
                name: "IX_media_EventId",
                table: "media",
                column: "EventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_media_EventId",
                table: "media");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "media",
                newName: "MediaUid");

            migrationBuilder.CreateIndex(
                name: "IX_media_EventId_MediaUid",
                table: "media",
                columns: new[] { "EventId", "MediaUid" },
                unique: true);
        }
    }
}
