using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedtableRemediations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Remediations",
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
                    BusinessEntityId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    RemediationPlanDetail = table.Column<string>(nullable: true),
                    ActualClosureDate = table.Column<DateTime>(nullable: true),
                    ExpertReviewerId = table.Column<long>(nullable: true),
                    EntityApproverId = table.Column<long>(nullable: true),
                    ReviewerComment = table.Column<string>(nullable: true),
                    ApprovedTillDate = table.Column<DateTime>(nullable: true),
                    Signature = table.Column<string>(nullable: true),
                    AuthorityExpertReviewerId = table.Column<long>(nullable: true),
                    AuthorityApproverId = table.Column<long>(nullable: true),
                    ReviewComment = table.Column<string>(nullable: true),
                    NextReviewDate = table.Column<DateTime>(nullable: true),
                    AuthorityApprovedTillDate = table.Column<DateTime>(nullable: true),
                    Authoritysignature = table.Column<string>(nullable: true),
                    RemediationPlanStatusId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remediations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Remediations_AbpUsers_AuthorityApproverId",
                        column: x => x.AuthorityApproverId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Remediations_AbpUsers_AuthorityExpertReviewerId",
                        column: x => x.AuthorityExpertReviewerId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Remediations_BusinessEntities_BusinessEntityId",
                        column: x => x.BusinessEntityId,
                        principalTable: "BusinessEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Remediations_AbpUsers_EntityApproverId",
                        column: x => x.EntityApproverId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Remediations_AbpUsers_ExpertReviewerId",
                        column: x => x.ExpertReviewerId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Remediations_AbpDynamicParameterValues_RemediationPlanStatusId",
                        column: x => x.RemediationPlanStatusId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Remediations_AuthorityApproverId",
                table: "Remediations",
                column: "AuthorityApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_Remediations_AuthorityExpertReviewerId",
                table: "Remediations",
                column: "AuthorityExpertReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_Remediations_BusinessEntityId",
                table: "Remediations",
                column: "BusinessEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Remediations_EntityApproverId",
                table: "Remediations",
                column: "EntityApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_Remediations_ExpertReviewerId",
                table: "Remediations",
                column: "ExpertReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_Remediations_RemediationPlanStatusId",
                table: "Remediations",
                column: "RemediationPlanStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Remediations");
        }
    }
}
