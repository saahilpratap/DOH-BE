using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_vendorId_auditor_FindingReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                table: "FindingReports",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FindingReports_AuditorId",
                table: "FindingReports",
                column: "AuditorId");

            migrationBuilder.CreateIndex(
                name: "IX_FindingReports_VendorId",
                table: "FindingReports",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_FindingReports_AbpUsers_AuditorId",
                table: "FindingReports",
                column: "AuditorId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FindingReports_BusinessEntities_VendorId",
                table: "FindingReports",
                column: "VendorId",
                principalTable: "BusinessEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FindingReports_AbpUsers_AuditorId",
                table: "FindingReports");

            migrationBuilder.DropForeignKey(
                name: "FK_FindingReports_BusinessEntities_VendorId",
                table: "FindingReports");

            migrationBuilder.DropIndex(
                name: "IX_FindingReports_AuditorId",
                table: "FindingReports");

            migrationBuilder.DropIndex(
                name: "IX_FindingReports_VendorId",
                table: "FindingReports");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "FindingReports");
        }
    }
}
