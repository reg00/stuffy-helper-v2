using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StuffyHelper.Authorization.EntityFrameworkCore.Migrations
{
    public partial class AddFriendsIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_friends-requests_UserIdFrom",
                table: "friends-requests");

            migrationBuilder.DropIndex(
                name: "IX_friends_UserId",
                table: "friends");

            migrationBuilder.CreateIndex(
                name: "IX_friends-requests_UserIdFrom_UserIdTo",
                table: "friends-requests",
                columns: new[] { "UserIdFrom", "UserIdTo" });

            migrationBuilder.CreateIndex(
                name: "IX_friends_UserId_FriendId",
                table: "friends",
                columns: new[] { "UserId", "FriendId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_friends-requests_UserIdFrom_UserIdTo",
                table: "friends-requests");

            migrationBuilder.DropIndex(
                name: "IX_friends_UserId_FriendId",
                table: "friends");

            migrationBuilder.CreateIndex(
                name: "IX_friends-requests_UserIdFrom",
                table: "friends-requests",
                column: "UserIdFrom");

            migrationBuilder.CreateIndex(
                name: "IX_friends_UserId",
                table: "friends",
                column: "UserId");
        }
    }
}
