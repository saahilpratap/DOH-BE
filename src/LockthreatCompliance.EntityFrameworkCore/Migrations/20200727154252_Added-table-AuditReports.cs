using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedtableAuditReports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditReports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    AuditProjectId = table.Column<long>(nullable: false),
                    NumberofAuditors = table.Column<int>(nullable: true),
                    OnsiteRemote = table.Column<string>(nullable: true),
                    Desktop = table.Column<string>(nullable: true),
                    LeadAuditorId = table.Column<long>(nullable: true),
                    ClosureFinding = table.Column<string>(nullable: true),
                    AreaImprovement = table.Column<string>(nullable: true),
                    Recommendation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditReports_AuditProjects_AuditProjectId",
                        column: x => x.AuditProjectId,
                        principalTable: "AuditProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditReports_AbpUsers_LeadAuditorId",
                        column: x => x.LeadAuditorId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditReports_AuditProjectId",
                table: "AuditReports",
                column: "AuditProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditReports_LeadAuditorId",
                table: "AuditReports",
                column: "LeadAuditorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditReports");
        }
    }
}
