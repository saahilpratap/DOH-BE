using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class UpdatetableFindingreportaddedfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ActionResponseDate",
                table: "FindingReports",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateClosed",
                table: "FindingReports",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateFound",
                table: "FindingReports",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FindingStatusId",
                table: "FindingReports",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FindingReports_FindingStatusId",
                table: "FindingReports",
                column: "FindingStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_FindingReports_AbpDynamicParameterValues_FindingStatusId",
                table: "FindingReports",
                column: "FindingStatusId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FindingReports_AbpDynamicParameterValues_FindingStatusId",
                table: "FindingReports");

            migrationBuilder.DropIndex(
                name: "IX_FindingReports_FindingStatusId",
                table: "FindingReports");

            migrationBuilder.DropColumn(
                name: "ActionResponseDate",
                table: "FindingReports");

            migrationBuilder.DropColumn(
                name: "DateClosed",
                table: "FindingReports");

            migrationBuilder.DropColumn(
                name: "DateFound",
                table: "FindingReports");

            migrationBuilder.DropColumn(
                name: "FindingStatusId",
                table: "FindingReports");
        }
    }
}
