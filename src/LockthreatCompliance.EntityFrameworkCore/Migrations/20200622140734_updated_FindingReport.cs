using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updated_FindingReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FindingReports_AbpDynamicParameterValues_criticalityId",
                table: "FindingReports");

            migrationBuilder.RenameColumn(
                name: "criticalityId",
                table: "FindingReports",
                newName: "CriticalityId");

            migrationBuilder.RenameIndex(
                name: "IX_FindingReports_criticalityId",
                table: "FindingReports",
                newName: "IX_FindingReports_CriticalityId");

            migrationBuilder.AddForeignKey(
                name: "FK_FindingReports_AbpDynamicParameterValues_CriticalityId",
                table: "FindingReports",
                column: "CriticalityId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FindingReports_AbpDynamicParameterValues_CriticalityId",
                table: "FindingReports");

            migrationBuilder.RenameColumn(
                name: "CriticalityId",
                table: "FindingReports",
                newName: "criticalityId");

            migrationBuilder.RenameIndex(
                name: "IX_FindingReports_CriticalityId",
                table: "FindingReports",
                newName: "IX_FindingReports_criticalityId");

            migrationBuilder.AddForeignKey(
                name: "FK_FindingReports_AbpDynamicParameterValues_criticalityId",
                table: "FindingReports",
                column: "criticalityId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
