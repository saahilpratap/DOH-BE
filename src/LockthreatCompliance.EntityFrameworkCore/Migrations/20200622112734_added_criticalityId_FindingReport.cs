using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_criticalityId_FindingReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "criticalityId",
                table: "FindingReports",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FindingReports_criticalityId",
                table: "FindingReports",
                column: "criticalityId");

            migrationBuilder.AddForeignKey(
                name: "FK_FindingReports_AbpDynamicParameterValues_criticalityId",
                table: "FindingReports",
                column: "criticalityId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FindingReports_AbpDynamicParameterValues_criticalityId",
                table: "FindingReports");

            migrationBuilder.DropIndex(
                name: "IX_FindingReports_criticalityId",
                table: "FindingReports");

            migrationBuilder.DropColumn(
                name: "criticalityId",
                table: "FindingReports");
        }
    }
}
