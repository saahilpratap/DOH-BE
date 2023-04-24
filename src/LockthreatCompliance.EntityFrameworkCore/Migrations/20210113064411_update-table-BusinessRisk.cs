using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updatetableBusinessRisk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualClosureDate",
                table: "BusinessRisks");

            migrationBuilder.DropColumn(
                name: "CompletionDate",
                table: "BusinessRisks");

            migrationBuilder.DropColumn(
                name: "ExpectedClosureDate",
                table: "BusinessRisks");

            migrationBuilder.DropColumn(
                name: "IdentificationDate",
                table: "BusinessRisks");

            migrationBuilder.DropColumn(
                name: "RiskAssessmentDate",
                table: "BusinessRisks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ActualClosureDate",
                table: "BusinessRisks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletionDate",
                table: "BusinessRisks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedClosureDate",
                table: "BusinessRisks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "IdentificationDate",
                table: "BusinessRisks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "RiskAssessmentDate",
                table: "BusinessRisks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
