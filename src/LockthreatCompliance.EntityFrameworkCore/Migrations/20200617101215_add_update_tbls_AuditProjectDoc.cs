using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class add_update_tbls_AuditProjectDoc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditProjectAuthoritativeDocuments_AuditProjects_AuditProjectId1",
                table: "AuditProjectAuthoritativeDocuments");

            migrationBuilder.DropIndex(
                name: "IX_AuditProjectAuthoritativeDocuments_AuditProjectId1",
                table: "AuditProjectAuthoritativeDocuments");

            migrationBuilder.DropColumn(
                name: "AuditProjectId1",
                table: "AuditProjectAuthoritativeDocuments");

            migrationBuilder.AlterColumn<long>(
                name: "AuditProjectId",
                table: "AuditProjectAuthoritativeDocuments",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_AuditProjectAuthoritativeDocuments_AuditProjectId",
                table: "AuditProjectAuthoritativeDocuments",
                column: "AuditProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditProjectAuthoritativeDocuments_AuditProjects_AuditProjectId",
                table: "AuditProjectAuthoritativeDocuments",
                column: "AuditProjectId",
                principalTable: "AuditProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditProjectAuthoritativeDocuments_AuditProjects_AuditProjectId",
                table: "AuditProjectAuthoritativeDocuments");

            migrationBuilder.DropIndex(
                name: "IX_AuditProjectAuthoritativeDocuments_AuditProjectId",
                table: "AuditProjectAuthoritativeDocuments");

            migrationBuilder.AlterColumn<int>(
                name: "AuditProjectId",
                table: "AuditProjectAuthoritativeDocuments",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "AuditProjectId1",
                table: "AuditProjectAuthoritativeDocuments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditProjectAuthoritativeDocuments_AuditProjectId1",
                table: "AuditProjectAuthoritativeDocuments",
                column: "AuditProjectId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditProjectAuthoritativeDocuments_AuditProjects_AuditProjectId1",
                table: "AuditProjectAuthoritativeDocuments",
                column: "AuditProjectId1",
                principalTable: "AuditProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
