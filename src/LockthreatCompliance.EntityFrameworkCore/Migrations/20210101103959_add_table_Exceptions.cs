using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class add_table_Exceptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "Exceptions",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "Exceptions",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "Exceptions",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Exceptions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "Exceptions",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "Exceptions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Exceptions");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "Exceptions");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "Exceptions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Exceptions");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "Exceptions");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "Exceptions");
        }
    }
}
