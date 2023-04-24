using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class Added_IRM_Relation_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IRMRelations",
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
                    ReviewComments = table.Column<string>(maxLength: 500, nullable: true),
                    ApprovalDate = table.Column<DateTime>(nullable: true),
                    Signature = table.Column<string>(maxLength: 9999, nullable: true),
                    IncidentId = table.Column<int>(nullable: true),
                    ExceptionId = table.Column<int>(nullable: true),
                    BusinessRiskId = table.Column<int>(nullable: true),
                    FindingReportId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IRMRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IRMRelations_BusinessRisks_BusinessRiskId",
                        column: x => x.BusinessRiskId,
                        principalTable: "BusinessRisks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IRMRelations_Exceptions_ExceptionId",
                        column: x => x.ExceptionId,
                        principalTable: "Exceptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IRMRelations_FindingReports_FindingReportId",
                        column: x => x.FindingReportId,
                        principalTable: "FindingReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IRMRelations_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IRMUsersRelation",
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
                    EntityReviewerId = table.Column<long>(nullable: true),
                    EntityApproverId = table.Column<long>(nullable: true),
                    AuthorityReviewerId = table.Column<long>(nullable: true),
                    AuthorityApproverId = table.Column<long>(nullable: true),
                    IRMRelationId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IRMUsersRelation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IRMUsersRelation_AbpUsers_AuthorityApproverId",
                        column: x => x.AuthorityApproverId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IRMUsersRelation_AbpUsers_AuthorityReviewerId",
                        column: x => x.AuthorityReviewerId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IRMUsersRelation_AbpUsers_EntityApproverId",
                        column: x => x.EntityApproverId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IRMUsersRelation_AbpUsers_EntityReviewerId",
                        column: x => x.EntityReviewerId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IRMUsersRelation_IRMRelations_IRMRelationId",
                        column: x => x.IRMRelationId,
                        principalTable: "IRMRelations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IRMRelations_BusinessRiskId",
                table: "IRMRelations",
                column: "BusinessRiskId");

            migrationBuilder.CreateIndex(
                name: "IX_IRMRelations_ExceptionId",
                table: "IRMRelations",
                column: "ExceptionId");

            migrationBuilder.CreateIndex(
                name: "IX_IRMRelations_FindingReportId",
                table: "IRMRelations",
                column: "FindingReportId");

            migrationBuilder.CreateIndex(
                name: "IX_IRMRelations_IncidentId",
                table: "IRMRelations",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_IRMUsersRelation_AuthorityApproverId",
                table: "IRMUsersRelation",
                column: "AuthorityApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_IRMUsersRelation_AuthorityReviewerId",
                table: "IRMUsersRelation",
                column: "AuthorityReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_IRMUsersRelation_EntityApproverId",
                table: "IRMUsersRelation",
                column: "EntityApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_IRMUsersRelation_EntityReviewerId",
                table: "IRMUsersRelation",
                column: "EntityReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_IRMUsersRelation_IRMRelationId",
                table: "IRMUsersRelation",
                column: "IRMRelationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IRMUsersRelation");

            migrationBuilder.DropTable(
                name: "IRMRelations");

            migrationBuilder.DropColumn(
                name: "IsRemediation",
                table: "Remediations");
        }
    }
}
