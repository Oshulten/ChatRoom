using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class NameChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Spaces_DbSpaceId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "DbSpaceId",
                table: "Users",
                newName: "SpaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_DbSpaceId",
                table: "Users",
                newName: "IX_Users_SpaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Spaces_SpaceId",
                table: "Users",
                column: "SpaceId",
                principalTable: "Spaces",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Spaces_SpaceId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "SpaceId",
                table: "Users",
                newName: "DbSpaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_SpaceId",
                table: "Users",
                newName: "IX_Users_DbSpaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Spaces_DbSpaceId",
                table: "Users",
                column: "DbSpaceId",
                principalTable: "Spaces",
                principalColumn: "Id");
        }
    }
}
