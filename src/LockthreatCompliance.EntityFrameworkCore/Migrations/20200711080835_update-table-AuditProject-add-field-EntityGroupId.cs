using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updatetableAuditProjectaddfieldEntityGroupId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EntityGroupId",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditProjects_EntityGroupId",
                table: "AuditProjects",
                column: "EntityGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditProjects_EntityGroups_EntityGroupId",
                table: "AuditProjects",
                column: "EntityGroupId",
                principalTable: "EntityGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditProjects_EntityGroups_EntityGroupId",
                table: "AuditProjects");

            migrationBuilder.DropIndex(
                name: "IX_AuditProjects_EntityGroupId",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "EntityGroupId",
                table: "AuditProjects");
        }
    }
}
