using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedfieldForAuditProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateofReleasingReauditOne",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateofReleasingReauditTwo",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DaysTimeline",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EvidenceSharedDateOne",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EvidenceSharedDateTwo",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EvidenceStatusId",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EvidenceSubmTimeiClosedId",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OutcomeReportReleasedDate",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OverallStatusId",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentDetailsId",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReAuditScoreOne",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReAuditScoreTwo",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditProjects_EvidenceStatusId",
                table: "AuditProjects",
                column: "EvidenceStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditProjects_EvidenceSubmTimeiClosedId",
                table: "AuditProjects",
                column: "EvidenceSubmTimeiClosedId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditProjects_OverallStatusId",
                table: "AuditProjects",
                column: "OverallStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditProjects_PaymentDetailsId",
                table: "AuditProjects",
                column: "PaymentDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditProjects_AbpDynamicParameterValues_EvidenceStatusId",
                table: "AuditProjects",
                column: "EvidenceStatusId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditProjects_AbpDynamicParameterValues_EvidenceSubmTimeiClosedId",
                table: "AuditProjects",
                column: "EvidenceSubmTimeiClosedId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditProjects_AbpDynamicParameterValues_OverallStatusId",
                table: "AuditProjects",
                column: "OverallStatusId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditProjects_AbpDynamicParameterValues_PaymentDetailsId",
                table: "AuditProjects",
                column: "PaymentDetailsId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditProjects_AbpDynamicParameterValues_EvidenceStatusId",
                table: "AuditProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditProjects_AbpDynamicParameterValues_EvidenceSubmTimeiClosedId",
                table: "AuditProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditProjects_AbpDynamicParameterValues_OverallStatusId",
                table: "AuditProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditProjects_AbpDynamicParameterValues_PaymentDetailsId",
                table: "AuditProjects");

            migrationBuilder.DropIndex(
                name: "IX_AuditProjects_EvidenceStatusId",
                table: "AuditProjects");

            migrationBuilder.DropIndex(
                name: "IX_AuditProjects_EvidenceSubmTimeiClosedId",
                table: "AuditProjects");

            migrationBuilder.DropIndex(
                name: "IX_AuditProjects_OverallStatusId",
                table: "AuditProjects");

            migrationBuilder.DropIndex(
                name: "IX_AuditProjects_PaymentDetailsId",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "DateofReleasingReauditOne",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "DateofReleasingReauditTwo",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "DaysTimeline",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "EvidenceSharedDateOne",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "EvidenceSharedDateTwo",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "EvidenceStatusId",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "EvidenceSubmTimeiClosedId",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "OutcomeReportReleasedDate",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "OverallStatusId",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "PaymentDetailsId",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "ReAuditScoreOne",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "ReAuditScoreTwo",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "AuditProjects");
        }
    }
}
