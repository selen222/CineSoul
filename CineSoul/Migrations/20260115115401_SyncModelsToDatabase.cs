using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineSoul.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelsToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PosterPath",
                table: "Ratings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_WatchHistories_MovieId",
                table: "WatchHistories",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_WatchHistories_Movies_MovieId",
                table: "WatchHistories",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WatchHistories_Movies_MovieId",
                table: "WatchHistories");

            migrationBuilder.DropIndex(
                name: "IX_WatchHistories_MovieId",
                table: "WatchHistories");

            migrationBuilder.DropColumn(
                name: "PosterPath",
                table: "Ratings");
        }
    }
}
