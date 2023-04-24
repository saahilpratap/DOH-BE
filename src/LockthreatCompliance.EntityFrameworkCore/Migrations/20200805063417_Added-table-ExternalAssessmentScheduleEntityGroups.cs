using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedtableExternalAssessmentScheduleEntityGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalAssessmentScheduleEntityGroups",
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
                    ExternalAssessmentScheduleId = table.Column<long>(nullable: true),
                    EntityGroupId = table.Column<int>(nullable: true),
                    BusinessEntityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalAssessmentScheduleEntityGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalAssessmentScheduleEntityGroups_BusinessEntities_BusinessEntityId",
                        column: x => x.BusinessEntityId,
                        principalTable: "BusinessEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalAssessmentScheduleEntityGroups_EntityGroups_EntityGroupId",
                        column: x => x.EntityGroupId,
                        principalTable: "EntityGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalAssessmentScheduleEntityGroups_ExternalAssessmentSchedules_ExternalAssessmentScheduleId",
                        column: x => x.ExternalAssessmentScheduleId,
                        principalTable: "ExternalAssessmentSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessmentScheduleEntityGroups_BusinessEntityId",
                table: "ExternalAssessmentScheduleEntityGroups",
                column: "BusinessEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessmentScheduleEntityGroups_EntityGroupId",
                table: "ExternalAssessmentScheduleEntityGroups",
                column: "EntityGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessmentScheduleEntityGroups_ExternalAssessmentScheduleId",
                table: "ExternalAssessmentScheduleEntityGroups",
                column: "ExternalAssessmentScheduleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalAssessmentScheduleEntityGroups");
        }
    }
}
