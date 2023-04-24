using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_auditProject_tble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AuditProjectId",
                table: "DocumentPaths",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuditProjects",
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
                    AuditTitle = table.Column<string>(nullable: true),
                    FiscalYear = table.Column<string>(nullable: true),
                    AuditScope = table.Column<string>(maxLength: 9999, nullable: true),
                    AuditObjective = table.Column<string>(maxLength: 9999, nullable: true),
                    AuditAreaId = table.Column<int>(nullable: true),
                    AuditTypeId = table.Column<int>(nullable: true),
                    AuditStageId = table.Column<int>(nullable: true),
                    AuditStatusId = table.Column<int>(nullable: true),
                    AuditCriteria = table.Column<string>(maxLength: 9999, nullable: true),
                    AuditMamangerId = table.Column<long>(nullable: true),
                    AuditManagerId = table.Column<long>(nullable: true),
                    AuditCoordinatorId = table.Column<long>(nullable: true),
                    LeadAuditorId = table.Column<long>(nullable: true),
                    LeadAuditeeId = table.Column<long>(nullable: true),
                    AuditorTeam = table.Column<string>(nullable: true),
                    AuditeeTeam = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    AuditDuration = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditProjects_AbpDynamicParameterValues_AuditAreaId",
                        column: x => x.AuditAreaId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditProjects_AbpUsers_AuditCoordinatorId",
                        column: x => x.AuditCoordinatorId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditProjects_AbpUsers_AuditManagerId",
                        column: x => x.AuditManagerId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditProjects_AbpDynamicParameterValues_AuditStageId",
                        column: x => x.AuditStageId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditProjects_AbpDynamicParameterValues_AuditStatusId",
                        column: x => x.AuditStatusId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditProjects_AbpDynamicParameterValues_AuditTypeId",
                        column: x => x.AuditTypeId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditProjects_AbpUsers_LeadAuditeeId",
                        column: x => x.LeadAuditeeId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditProjects_AbpUsers_LeadAuditorId",
                        column: x => x.LeadAuditorId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPaths_AuditProjectId",
                table: "DocumentPaths",
                column: "AuditProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditProjects_AuditAreaId",
                table: "AuditProjects",
                column: "AuditAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditProjects_AuditCoordinatorId",
                table: "AuditProjects",
                column: "AuditCoordinatorId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditProjects_AuditManagerId",
                table: "AuditProjects",
                column: "AuditManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditProjects_AuditStageId",
                table: "AuditProjects",
                column: "AuditStageId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditProjects_AuditStatusId",
                table: "AuditProjects",
                column: "AuditStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditProjects_AuditTypeId",
                table: "AuditProjects",
                column: "AuditTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditProjects_LeadAuditeeId",
                table: "AuditProjects",
                column: "LeadAuditeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditProjects_LeadAuditorId",
                table: "AuditProjects",
                column: "LeadAuditorId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentPaths_AuditProjects_AuditProjectId",
                table: "DocumentPaths",
                column: "AuditProjectId",
                principalTable: "AuditProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentPaths_AuditProjects_AuditProjectId",
                table: "DocumentPaths");

            migrationBuilder.DropTable(
                name: "AuditProjects");

            migrationBuilder.DropIndex(
                name: "IX_DocumentPaths_AuditProjectId",
                table: "DocumentPaths");

            migrationBuilder.DropColumn(
                name: "AuditProjectId",
                table: "DocumentPaths");
        }
    }
}
