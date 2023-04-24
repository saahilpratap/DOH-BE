using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_AuditProjectFK_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AuditProjectId",
                table: "AuditTemplates",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AuditProjectId",
                table: "AuditProcedures",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AuditProjectId",
                table: "AuditMeetings",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AuditProjectId",
                table: "AuditChecklists",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditTemplates_AuditProjectId",
                table: "AuditTemplates",
                column: "AuditProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditProcedures_AuditProjectId",
                table: "AuditProcedures",
                column: "AuditProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditMeetings_AuditProjectId",
                table: "AuditMeetings",
                column: "AuditProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditChecklists_AuditProjectId",
                table: "AuditChecklists",
                column: "AuditProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditChecklists_AuditProjects_AuditProjectId",
                table: "AuditChecklists",
                column: "AuditProjectId",
                principalTable: "AuditProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditMeetings_AuditProjects_AuditProjectId",
                table: "AuditMeetings",
                column: "AuditProjectId",
                principalTable: "AuditProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditProcedures_AuditProjects_AuditProjectId",
                table: "AuditProcedures",
                column: "AuditProjectId",
                principalTable: "AuditProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditTemplates_AuditProjects_AuditProjectId",
                table: "AuditTemplates",
                column: "AuditProjectId",
                principalTable: "AuditProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditChecklists_AuditProjects_AuditProjectId",
                table: "AuditChecklists");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditMeetings_AuditProjects_AuditProjectId",
                table: "AuditMeetings");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditProcedures_AuditProjects_AuditProjectId",
                table: "AuditProcedures");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditTemplates_AuditProjects_AuditProjectId",
                table: "AuditTemplates");

            migrationBuilder.DropIndex(
                name: "IX_AuditTemplates_AuditProjectId",
                table: "AuditTemplates");

            migrationBuilder.DropIndex(
                name: "IX_AuditProcedures_AuditProjectId",
                table: "AuditProcedures");

            migrationBuilder.DropIndex(
                name: "IX_AuditMeetings_AuditProjectId",
                table: "AuditMeetings");

            migrationBuilder.DropIndex(
                name: "IX_AuditChecklists_AuditProjectId",
                table: "AuditChecklists");

            migrationBuilder.DropColumn(
                name: "AuditProjectId",
                table: "AuditTemplates");

            migrationBuilder.DropColumn(
                name: "AuditProjectId",
                table: "AuditProcedures");

            migrationBuilder.DropColumn(
                name: "AuditProjectId",
                table: "AuditMeetings");

            migrationBuilder.DropColumn(
                name: "AuditProjectId",
                table: "AuditChecklists");
        }
    }
}
