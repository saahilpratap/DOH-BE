using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class UpdatetableAuditProjectAuthoritativeDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "AuditProjectAuthoritativeDocuments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "AuditProjectAuthoritativeDocuments",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "AuditProjectAuthoritativeDocuments",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "AuditProjectAuthoritativeDocuments",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AuditProjectAuthoritativeDocuments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "AuditProjectAuthoritativeDocuments",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "AuditProjectAuthoritativeDocuments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "AuditProjectAuthoritativeDocuments");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "AuditProjectAuthoritativeDocuments");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "AuditProjectAuthoritativeDocuments");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "AuditProjectAuthoritativeDocuments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AuditProjectAuthoritativeDocuments");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "AuditProjectAuthoritativeDocuments");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "AuditProjectAuthoritativeDocuments");
        }
    }
}
