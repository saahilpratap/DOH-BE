using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_AuditMeetings_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AuditMeetingId",
                table: "DocumentPaths",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuditMeetings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    MeetingTitle = table.Column<string>(nullable: true),
                    AppEntityType = table.Column<int>(nullable: false),
                    EditorData = table.Column<string>(maxLength: 9999, nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditMeetings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPaths_AuditMeetingId",
                table: "DocumentPaths",
                column: "AuditMeetingId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentPaths_AuditMeetings_AuditMeetingId",
                table: "DocumentPaths",
                column: "AuditMeetingId",
                principalTable: "AuditMeetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentPaths_AuditMeetings_AuditMeetingId",
                table: "DocumentPaths");

            migrationBuilder.DropTable(
                name: "AuditMeetings");

            migrationBuilder.DropIndex(
                name: "IX_DocumentPaths_AuditMeetingId",
                table: "DocumentPaths");

            migrationBuilder.DropColumn(
                name: "AuditMeetingId",
                table: "DocumentPaths");
        }
    }
}
