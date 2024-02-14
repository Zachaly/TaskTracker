using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTracker.Database.Migrations
{
    /// <inheritdoc />
    public partial class add_space_user_permissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpaceUserPermissions",
                columns: table => new
                {
                    SpaceId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CanAddUsers = table.Column<bool>(type: "bit", nullable: false),
                    CanRemoveUsers = table.Column<bool>(type: "bit", nullable: false),
                    CanChangePermissions = table.Column<bool>(type: "bit", nullable: false),
                    CanModifyLists = table.Column<bool>(type: "bit", nullable: false),
                    CanRemoveLists = table.Column<bool>(type: "bit", nullable: false),
                    CanModifyTasks = table.Column<bool>(type: "bit", nullable: false),
                    CanRemoveTasks = table.Column<bool>(type: "bit", nullable: false),
                    CanAssignTaskUsers = table.Column<bool>(type: "bit", nullable: false),
                    CanModifyStatusGroups = table.Column<bool>(type: "bit", nullable: false),
                    CanModifySpace = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpaceUserPermissions", x => new { x.UserId, x.SpaceId });
                    table.ForeignKey(
                        name: "FK_SpaceUserPermissions_UserSpaces_SpaceId",
                        column: x => x.SpaceId,
                        principalTable: "UserSpaces",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SpaceUserPermissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpaceUserPermissions_SpaceId",
                table: "SpaceUserPermissions",
                column: "SpaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpaceUserPermissions");
        }
    }
}
