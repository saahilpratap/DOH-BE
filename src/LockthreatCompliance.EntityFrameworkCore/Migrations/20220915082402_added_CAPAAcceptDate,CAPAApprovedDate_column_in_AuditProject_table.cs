using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_CAPAAcceptDateCAPAApprovedDate_column_in_AuditProject_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CAPAAcceptDate",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CAPAApprovedDate",
                table: "AuditProjects",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CAPAAcceptDate",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "CAPAApprovedDate",
                table: "AuditProjects");
        }
    }
}
