using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedtableAuditProjectStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditProjectStatus",
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
                    AuditProjectId = table.Column<long>(nullable: false),
                    StatusId = table.Column<int>(nullable: true),
                    UserActedId = table.Column<long>(nullable: true),
                    ActionDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditProjectStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditProjectStatus_AuditProjects_AuditProjectId",
                        column: x => x.AuditProjectId,
                        principalTable: "AuditProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditProjectStatus_AbpDynamicParameterValues_StatusId",
                        column: x => x.StatusId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditProjectStatus_AbpUsers_UserActedId",
                        column: x => x.UserActedId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditProjectStatus_AuditProjectId",
                table: "AuditProjectStatus",
                column: "AuditProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditProjectStatus_StatusId",
                table: "AuditProjectStatus",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditProjectStatus_UserActedId",
                table: "AuditProjectStatus",
                column: "UserActedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditProjectStatus");
        }
    }
}
