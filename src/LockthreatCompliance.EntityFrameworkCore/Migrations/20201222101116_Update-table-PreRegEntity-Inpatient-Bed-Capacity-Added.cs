using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class UpdatetablePreRegEntityInpatientBedCapacityAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "PreRegEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "INPATIENT_BED_CAPACITY",
                table: "PreRegEntities",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "IssueDate",
                table: "PreRegEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "PreRegEntities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Group",
                table: "PreRegEntities");

            migrationBuilder.DropColumn(
                name: "INPATIENT_BED_CAPACITY",
                table: "PreRegEntities");

            migrationBuilder.DropColumn(
                name: "IssueDate",
                table: "PreRegEntities");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "PreRegEntities");
        }
    }
}
