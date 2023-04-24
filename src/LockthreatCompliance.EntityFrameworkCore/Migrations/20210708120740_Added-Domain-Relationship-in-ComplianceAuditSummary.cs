using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedDomainRelationshipinComplianceAuditSummary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ComplianceAuditSummarys_DomainId",
                table: "ComplianceAuditSummarys",
                column: "DomainId");

            migrationBuilder.AddForeignKey(
                name: "FK_ComplianceAuditSummarys_Domains_DomainId",
                table: "ComplianceAuditSummarys",
                column: "DomainId",
                principalTable: "Domains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComplianceAuditSummarys_Domains_DomainId",
                table: "ComplianceAuditSummarys");

            migrationBuilder.DropIndex(
                name: "IX_ComplianceAuditSummarys_DomainId",
                table: "ComplianceAuditSummarys");
        }
    }
}
