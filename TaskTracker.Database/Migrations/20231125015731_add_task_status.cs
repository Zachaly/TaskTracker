using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTracker.Database.Migrations
{
    /// <inheritdoc />
    public partial class add_task_status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ListId",
                table: "Tasks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StatusId",
                table: "Tasks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TaskStatusGroupId",
                table: "TaskLists",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "TaskStatusGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskStatusGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskStatusGroups_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserTaskStatuses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    GroupId = table.Column<long>(type: "bigint", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTaskStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTaskStatuses_TaskStatusGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "TaskStatusGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_StatusId",
                table: "Tasks",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskLists_TaskStatusGroupId",
                table: "TaskLists",
                column: "TaskStatusGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskStatusGroups_UserId",
                table: "TaskStatusGroups",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTaskStatuses_GroupId",
                table: "UserTaskStatuses",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLists_TaskStatusGroups_TaskStatusGroupId",
                table: "TaskLists",
                column: "TaskStatusGroupId",
                principalTable: "TaskStatusGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_UserTaskStatuses_StatusId",
                table: "Tasks",
                column: "StatusId",
                principalTable: "UserTaskStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskLists_TaskStatusGroups_TaskStatusGroupId",
                table: "TaskLists");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_UserTaskStatuses_StatusId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "UserTaskStatuses");

            migrationBuilder.DropTable(
                name: "TaskStatusGroups");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_StatusId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_TaskLists_TaskStatusGroupId",
                table: "TaskLists");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskStatusGroupId",
                table: "TaskLists");

            migrationBuilder.AlterColumn<long>(
                name: "ListId",
                table: "Tasks",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
