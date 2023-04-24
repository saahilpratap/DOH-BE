using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class RemovefiledAuditProjectIdCertificationProposals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CertificationProposals_AuditProjects_AuditProjectId1",
                table: "CertificationProposals");

            migrationBuilder.DropIndex(
                name: "IX_CertificationProposals_AuditProjectId1",
                table: "CertificationProposals");

            migrationBuilder.DropColumn(
                name: "AuditProjectId",
                table: "CertificationProposals");

            migrationBuilder.DropColumn(
                name: "AuditProjectId1",
                table: "CertificationProposals");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuditProjectId",
                table: "CertificationProposals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "AuditProjectId1",
                table: "CertificationProposals",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CertificationProposals_AuditProjectId1",
                table: "CertificationProposals",
                column: "AuditProjectId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CertificationProposals_AuditProjects_AuditProjectId1",
                table: "CertificationProposals",
                column: "AuditProjectId1",
                principalTable: "AuditProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
