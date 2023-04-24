using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedtableAuditSurviellanceEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditSurviellanceEntities",
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
                    AuditSurviellanceProjectId = table.Column<long>(nullable: false),
                    AuditTypeId = table.Column<int>(nullable: true),
                    BusinessEntityId = table.Column<int>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    ManDays = table.Column<int>(nullable: true),
                    SamplingSite = table.Column<string>(nullable: true),
                    Process = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditSurviellanceEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditSurviellanceEntities_AuditSurviellanceProjects_AuditSurviellanceProjectId",
                        column: x => x.AuditSurviellanceProjectId,
                        principalTable: "AuditSurviellanceProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditSurviellanceEntities_AbpDynamicParameterValues_AuditTypeId",
                        column: x => x.AuditTypeId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditSurviellanceEntities_BusinessEntities_BusinessEntityId",
                        column: x => x.BusinessEntityId,
                        principalTable: "BusinessEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditSurviellanceEntities_AuditSurviellanceProjectId",
                table: "AuditSurviellanceEntities",
                column: "AuditSurviellanceProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditSurviellanceEntities_AuditTypeId",
                table: "AuditSurviellanceEntities",
                column: "AuditTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditSurviellanceEntities_BusinessEntityId",
                table: "AuditSurviellanceEntities",
                column: "BusinessEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditSurviellanceEntities");
        }
    }
}
