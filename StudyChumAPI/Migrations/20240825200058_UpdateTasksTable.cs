using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyChumAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTasksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tasks_users_UserID",
                table: "tasks");

            migrationBuilder.DropIndex(
                name: "IX_tasks_UserID",
                table: "tasks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tasks_UserID",
                table: "tasks",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tasks_users_UserID",
                table: "tasks",
                column: "UserID",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
