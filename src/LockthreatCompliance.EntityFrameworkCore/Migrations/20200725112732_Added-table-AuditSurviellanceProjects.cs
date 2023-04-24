using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedtableAuditSurviellanceProjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditSurviellanceProjects",
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
                    Date = table.Column<DateTime>(nullable: true),
                    PlannedById = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditSurviellanceProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditSurviellanceProjects_AuditProjects_AuditProjectId",
                        column: x => x.AuditProjectId,
                        principalTable: "AuditProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditSurviellanceProjects_AbpUsers_PlannedById",
                        column: x => x.PlannedById,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditSurviellanceProjects_AuditProjectId",
                table: "AuditSurviellanceProjects",
                column: "AuditProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditSurviellanceProjects_PlannedById",
                table: "AuditSurviellanceProjects",
                column: "PlannedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditSurviellanceProjects");
        }
    }
}
