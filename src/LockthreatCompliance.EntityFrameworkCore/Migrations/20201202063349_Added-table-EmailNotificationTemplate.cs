using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedtableEmailNotificationTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailNotificationTemplates",
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
                    WorkFlowPageId = table.Column<long>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    AuditStatusId = table.Column<int>(nullable: true),
                    EmailBody = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailNotificationTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailNotificationTemplates_AbpDynamicParameterValues_AuditStatusId",
                        column: x => x.AuditStatusId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmailNotificationTemplates_WorkFlowPage_WorkFlowPageId",
                        column: x => x.WorkFlowPageId,
                        principalTable: "WorkFlowPage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailNotificationTemplates_AuditStatusId",
                table: "EmailNotificationTemplates",
                column: "AuditStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailNotificationTemplates_WorkFlowPageId",
                table: "EmailNotificationTemplates",
                column: "WorkFlowPageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailNotificationTemplates");
        }
    }
}
