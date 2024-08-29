using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class BidirectionalRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Spaces_SpaceId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SpaceId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SpaceId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "SpaceUser",
                columns: table => new
                {
                    MembersId = table.Column<int>(type: "int", nullable: false),
                    SpacesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpaceUser", x => new { x.MembersId, x.SpacesId });
                    table.ForeignKey(
                        name: "FK_SpaceUser_Spaces_SpacesId",
                        column: x => x.SpacesId,
                        principalTable: "Spaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpaceUser_Users_MembersId",
                        column: x => x.MembersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpaceUser_SpacesId",
                table: "SpaceUser",
                column: "SpacesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpaceUser");

            migrationBuilder.AddColumn<int>(
                name: "SpaceId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_SpaceId",
                table: "Users",
                column: "SpaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Spaces_SpaceId",
                table: "Users",
                column: "SpaceId",
                principalTable: "Spaces",
                principalColumn: "Id");
        }
    }
}
