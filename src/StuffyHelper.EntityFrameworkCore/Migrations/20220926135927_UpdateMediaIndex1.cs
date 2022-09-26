using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StuffyHelper.EntityFrameworkCore.Migrations
{
    public partial class UpdateMediaIndex1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_media_EventId",
                table: "media");

            migrationBuilder.DropIndex(
                name: "IX_media_MediaUid",
                table: "media");

            migrationBuilder.CreateIndex(
                name: "IX_media_EventId_MediaUid",
                table: "media",
                columns: new[] { "EventId", "MediaUid" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_media_EventId_MediaUid",
                table: "media");

            migrationBuilder.CreateIndex(
                name: "IX_media_EventId",
                table: "media",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_media_MediaUid",
                table: "media",
                column: "MediaUid",
                unique: true);
        }
    }
}
