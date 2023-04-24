using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_FindingCloseDateExternalAssessmentIdExternalAssessmentResponseType_column_in_FindingReport_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExternalAssessmentId",
                table: "FindingReports",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExternalAssessmentResponseType",
                table: "FindingReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FindingCloseDate",
                table: "FindingReports",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FindingReports_ExternalAssessmentId",
                table: "FindingReports",
                column: "ExternalAssessmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_FindingReports_ExternalAssessments_ExternalAssessmentId",
                table: "FindingReports",
                column: "ExternalAssessmentId",
                principalTable: "ExternalAssessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FindingReports_ExternalAssessments_ExternalAssessmentId",
                table: "FindingReports");

            migrationBuilder.DropIndex(
                name: "IX_FindingReports_ExternalAssessmentId",
                table: "FindingReports");

            migrationBuilder.DropColumn(
                name: "ExternalAssessmentId",
                table: "FindingReports");

            migrationBuilder.DropColumn(
                name: "ExternalAssessmentResponseType",
                table: "FindingReports");

            migrationBuilder.DropColumn(
                name: "FindingCloseDate",
                table: "FindingReports");
        }
    }
}
