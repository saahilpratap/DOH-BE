using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedtableInternalAssessmentScheduleBusinessEntitys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InternalAssessmentScheduleBusinessEntitys",
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
                    InternalAssessmentScheduleDetailId = table.Column<int>(nullable: true),
                    EntityGroupId = table.Column<int>(nullable: true),
                    BusinessEntityId = table.Column<int>(nullable: true),
                    ExtGenerated = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalAssessmentScheduleBusinessEntitys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InternalAssessmentScheduleBusinessEntitys_BusinessEntities_BusinessEntityId",
                        column: x => x.BusinessEntityId,
                        principalTable: "BusinessEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InternalAssessmentScheduleBusinessEntitys_EntityGroups_EntityGroupId",
                        column: x => x.EntityGroupId,
                        principalTable: "EntityGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InternalAssessmentScheduleBusinessEntitys_InternalAssessmentScheduleDetails_InternalAssessmentScheduleDetailId",
                        column: x => x.InternalAssessmentScheduleDetailId,
                        principalTable: "InternalAssessmentScheduleDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InternalAssessmentScheduleBusinessEntitys_BusinessEntityId",
                table: "InternalAssessmentScheduleBusinessEntitys",
                column: "BusinessEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalAssessmentScheduleBusinessEntitys_EntityGroupId",
                table: "InternalAssessmentScheduleBusinessEntitys",
                column: "EntityGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalAssessmentScheduleBusinessEntitys_InternalAssessmentScheduleDetailId",
                table: "InternalAssessmentScheduleBusinessEntitys",
                column: "InternalAssessmentScheduleDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InternalAssessmentScheduleBusinessEntitys");
        }
    }
}
