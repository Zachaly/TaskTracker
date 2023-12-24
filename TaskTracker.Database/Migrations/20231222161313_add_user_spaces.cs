using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTracker.Database.Migrations
{
    /// <inheritdoc />
    public partial class add_user_spaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SpaceId",
                table: "TaskLists",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "UserSpaces",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StatusGroupId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSpaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSpaces_TaskStatusGroups_StatusGroupId",
                        column: x => x.StatusGroupId,
                        principalTable: "TaskStatusGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSpaces_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskLists_SpaceId",
                table: "TaskLists",
                column: "SpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSpaces_OwnerId",
                table: "UserSpaces",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSpaces_StatusGroupId",
                table: "UserSpaces",
                column: "StatusGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLists_UserSpaces_SpaceId",
                table: "TaskLists",
                column: "SpaceId",
                principalTable: "UserSpaces",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskLists_UserSpaces_SpaceId",
                table: "TaskLists");

            migrationBuilder.DropTable(
                name: "UserSpaces");

            migrationBuilder.DropIndex(
                name: "IX_TaskLists_SpaceId",
                table: "TaskLists");

            migrationBuilder.DropColumn(
                name: "SpaceId",
                table: "TaskLists");
        }
    }
}
