using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updated_documentPath_tble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentPaths_AuditProjects_AuditProjectId",
                table: "DocumentPaths");

            migrationBuilder.DropIndex(
                name: "IX_DocumentPaths_AuditProjectId",
                table: "DocumentPaths");

            migrationBuilder.AlterColumn<int>(
                name: "AuditProjectId",
                table: "DocumentPaths",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AuditProjectId1",
                table: "DocumentPaths",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPaths_AuditProjectId1",
                table: "DocumentPaths",
                column: "AuditProjectId1");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentPaths_AuditProjects_AuditProjectId1",
                table: "DocumentPaths",
                column: "AuditProjectId1",
                principalTable: "AuditProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentPaths_AuditProjects_AuditProjectId1",
                table: "DocumentPaths");

            migrationBuilder.DropIndex(
                name: "IX_DocumentPaths_AuditProjectId1",
                table: "DocumentPaths");

            migrationBuilder.DropColumn(
                name: "AuditProjectId1",
                table: "DocumentPaths");

            migrationBuilder.AlterColumn<long>(
                name: "AuditProjectId",
                table: "DocumentPaths",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPaths_AuditProjectId",
                table: "DocumentPaths",
                column: "AuditProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentPaths_AuditProjects_AuditProjectId",
                table: "DocumentPaths",
                column: "AuditProjectId",
                principalTable: "AuditProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
