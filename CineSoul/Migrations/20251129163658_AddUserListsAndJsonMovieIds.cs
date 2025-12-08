using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineSoul.Migrations
{
    /// <inheritdoc />
    public partial class AddUserListsAndJsonMovieIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserList_AspNetUsers_OwnerId",
                table: "UserList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserList",
                table: "UserList");

            migrationBuilder.RenameTable(
                name: "UserList",
                newName: "UserLists");

            migrationBuilder.RenameIndex(
                name: "IX_UserList_OwnerId",
                table: "UserLists",
                newName: "IX_UserLists_OwnerId");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "UserLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLists",
                table: "UserLists",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLists_AspNetUsers_OwnerId",
                table: "UserLists",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLists_AspNetUsers_OwnerId",
                table: "UserLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLists",
                table: "UserLists");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "UserLists");

            migrationBuilder.RenameTable(
                name: "UserLists",
                newName: "UserList");

            migrationBuilder.RenameIndex(
                name: "IX_UserLists_OwnerId",
                table: "UserList",
                newName: "IX_UserList_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserList",
                table: "UserList",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserList_AspNetUsers_OwnerId",
                table: "UserList",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
