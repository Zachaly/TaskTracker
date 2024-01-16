using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTracker.Database.Migrations
{
    /// <inheritdoc />
    public partial class add_task_assigned_users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskAssignedUsers",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    TaskId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskAssignedUsers", x => new { x.TaskId, x.UserId });
                    table.ForeignKey(
                        name: "FK_TaskAssignedUsers_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskAssignedUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignedUsers_UserId",
                table: "TaskAssignedUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskAssignedUsers");
        }
    }
}
