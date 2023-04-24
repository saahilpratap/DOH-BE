using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_PatientAuthenticationPlatformLog_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientAuthenticationPlatformLogs",
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
                    TenantId = table.Column<int>(nullable: true),
                    LogUserId = table.Column<long>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    PAPId = table.Column<long>(nullable: false),
                    StatusId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAuthenticationPlatformLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientAuthenticationPlatformLogs_AbpUsers_LogUserId",
                        column: x => x.LogUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientAuthenticationPlatformLogs_PatientAuthenticationPlatforms_PAPId",
                        column: x => x.PAPId,
                        principalTable: "PatientAuthenticationPlatforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientAuthenticationPlatformLogs_AbpDynamicParameterValues_StatusId",
                        column: x => x.StatusId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientAuthenticationPlatformLogs_LogUserId",
                table: "PatientAuthenticationPlatformLogs",
                column: "LogUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAuthenticationPlatformLogs_PAPId",
                table: "PatientAuthenticationPlatformLogs",
                column: "PAPId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAuthenticationPlatformLogs_StatusId",
                table: "PatientAuthenticationPlatformLogs",
                column: "StatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientAuthenticationPlatformLogs");
        }
    }
}
