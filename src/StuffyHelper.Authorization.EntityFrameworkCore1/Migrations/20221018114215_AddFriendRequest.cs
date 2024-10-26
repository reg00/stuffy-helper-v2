using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StuffyHelper.Authorization.EntityFrameworkCore1.Migrations
{
    public partial class AddFriendRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "asp-net-friends",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserAId = table.Column<string>(type: "text", nullable: false),
                    UserBId = table.Column<string>(type: "text", nullable: false),
                    FriendsSince = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asp-net-friends", x => x.Id);
                    table.ForeignKey(
                        name: "FK_asp-net-friends_AspNetUsers_UserAId",
                        column: x => x.UserAId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_asp-net-friends_AspNetUsers_UserBId",
                        column: x => x.UserBId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "friends-requests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsComfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    UserIdFrom = table.Column<string>(type: "text", nullable: false),
                    UserIdTo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_friends-requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_friends-requests_AspNetUsers_UserIdFrom",
                        column: x => x.UserIdFrom,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_friends-requests_AspNetUsers_UserIdTo",
                        column: x => x.UserIdTo,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_asp-net-friends_UserAId",
                table: "asp-net-friends",
                column: "UserAId");

            migrationBuilder.CreateIndex(
                name: "IX_asp-net-friends_UserBId",
                table: "asp-net-friends",
                column: "UserBId");

            migrationBuilder.CreateIndex(
                name: "IX_friends-requests_UserIdFrom",
                table: "friends-requests",
                column: "UserIdFrom");

            migrationBuilder.CreateIndex(
                name: "IX_friends-requests_UserIdTo",
                table: "friends-requests",
                column: "UserIdTo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "asp-net-friends");

            migrationBuilder.DropTable(
                name: "friends-requests");
        }
    }
}
