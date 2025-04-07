using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class Updating_projectActivityTable_AddingProjectId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "projectId",
                table: "ProjectActivities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectActivities_projectId",
                table: "ProjectActivities",
                column: "projectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectActivities_Projects_projectId",
                table: "ProjectActivities",
                column: "projectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectActivities_Projects_projectId",
                table: "ProjectActivities");

            migrationBuilder.DropIndex(
                name: "IX_ProjectActivities_projectId",
                table: "ProjectActivities");

            migrationBuilder.DropColumn(
                name: "projectId",
                table: "ProjectActivities");
        }
    }
}
