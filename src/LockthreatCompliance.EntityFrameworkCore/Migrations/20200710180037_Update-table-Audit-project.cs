using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class UpdatetableAuditproject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StageAuditDuration",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StageEndDate",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StageStartDate",
                table: "AuditProjects",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StageAuditDuration",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "StageEndDate",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "StageStartDate",
                table: "AuditProjects");
        }
    }
}
