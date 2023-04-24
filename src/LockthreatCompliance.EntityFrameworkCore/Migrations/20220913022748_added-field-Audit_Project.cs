using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class addedfieldAudit_Project : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ActualAuditReportDate",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuditNewStatusId",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuditOutcomeReportId",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CAPAStatusId",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CAPAsubmissiondate",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date_of_releasing_1st_Revised",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date_of_releasing_2nd_Revised",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditProjects_AuditNewStatusId",
                table: "AuditProjects",
                column: "AuditNewStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditProjects_AuditOutcomeReportId",
                table: "AuditProjects",
                column: "AuditOutcomeReportId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditProjects_CAPAStatusId",
                table: "AuditProjects",
                column: "CAPAStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditProjects_AbpDynamicParameterValues_AuditNewStatusId",
                table: "AuditProjects",
                column: "AuditNewStatusId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditProjects_AbpDynamicParameterValues_AuditOutcomeReportId",
                table: "AuditProjects",
                column: "AuditOutcomeReportId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditProjects_AbpDynamicParameterValues_CAPAStatusId",
                table: "AuditProjects",
                column: "CAPAStatusId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditProjects_AbpDynamicParameterValues_AuditNewStatusId",
                table: "AuditProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditProjects_AbpDynamicParameterValues_AuditOutcomeReportId",
                table: "AuditProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditProjects_AbpDynamicParameterValues_CAPAStatusId",
                table: "AuditProjects");

            migrationBuilder.DropIndex(
                name: "IX_AuditProjects_AuditNewStatusId",
                table: "AuditProjects");

            migrationBuilder.DropIndex(
                name: "IX_AuditProjects_AuditOutcomeReportId",
                table: "AuditProjects");

            migrationBuilder.DropIndex(
                name: "IX_AuditProjects_CAPAStatusId",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "ActualAuditReportDate",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "AuditNewStatusId",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "AuditOutcomeReportId",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "CAPAStatusId",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "CAPAsubmissiondate",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "Date_of_releasing_1st_Revised",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "Date_of_releasing_2nd_Revised",
                table: "AuditProjects");
        }
    }
}
