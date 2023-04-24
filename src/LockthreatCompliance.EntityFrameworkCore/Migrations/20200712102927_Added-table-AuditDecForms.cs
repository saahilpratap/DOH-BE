using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedtableAuditDecForms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditDecForms",
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
                    DecisionDate = table.Column<DateTime>(nullable: true),
                    AuditProjectId = table.Column<long>(nullable: false),
                    EntityGroupId = table.Column<int>(nullable: false),
                    FacilityTypeId = table.Column<int>(nullable: false),
                    DocumentCheck = table.Column<string>(nullable: true),
                    OtherApplicable = table.Column<string>(nullable: true),
                    OutPutConClusion = table.Column<int>(nullable: false),
                    Judgement = table.Column<string>(nullable: true),
                    Decision = table.Column<string>(nullable: true),
                    DoHApprover = table.Column<string>(nullable: true),
                    AuditAgencyApprover = table.Column<string>(nullable: true),
                    DoHSign = table.Column<string>(nullable: true),
                    AuditVensign = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditDecForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditDecForms_AuditProjects_AuditProjectId",
                        column: x => x.AuditProjectId,
                        principalTable: "AuditProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditDecForms_EntityGroups_EntityGroupId",
                        column: x => x.EntityGroupId,
                        principalTable: "EntityGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditDecForms_FacilityTypes_FacilityTypeId",
                        column: x => x.FacilityTypeId,
                        principalTable: "FacilityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditDecForms_AuditProjectId",
                table: "AuditDecForms",
                column: "AuditProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditDecForms_EntityGroupId",
                table: "AuditDecForms",
                column: "EntityGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditDecForms_FacilityTypeId",
                table: "AuditDecForms",
                column: "FacilityTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditDecForms");
        }
    }
}
