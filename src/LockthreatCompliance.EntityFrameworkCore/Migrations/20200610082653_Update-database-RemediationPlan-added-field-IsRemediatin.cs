using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class UpdatedatabaseRemediationPlanaddedfieldIsRemediatin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedDate",
                table: "Remediations",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRemediation",
                table: "Remediations",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedDate",
                table: "Remediations");

            migrationBuilder.DropColumn(
                name: "IsRemediation",
                table: "Remediations");
        }
    }
}
