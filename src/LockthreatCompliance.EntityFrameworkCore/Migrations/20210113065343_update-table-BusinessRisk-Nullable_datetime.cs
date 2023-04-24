using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updatetableBusinessRiskNullable_datetime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ActualClosureDate",
                table: "BusinessRisks",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletionDate",
                table: "BusinessRisks",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedClosureDate",
                table: "BusinessRisks",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "IdentificationDate",
                table: "BusinessRisks",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RiskAssessmentDate",
                table: "BusinessRisks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
