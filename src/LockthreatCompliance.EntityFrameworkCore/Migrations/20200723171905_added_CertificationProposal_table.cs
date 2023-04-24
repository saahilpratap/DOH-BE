using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_CertificationProposal_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CertificationProposals",
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
                    AuditProjectId = table.Column<int>(nullable: false),
                    AuditProjectId1 = table.Column<long>(nullable: true),
                    ReferenceStandard = table.Column<int>(nullable: false),
                    EntityGroupId = table.Column<int>(nullable: true),
                    ProposalDate = table.Column<DateTime>(nullable: true),
                    Scope = table.Column<string>(nullable: true),
                    Grade = table.Column<string>(nullable: true),
                    TotalManDays = table.Column<int>(nullable: false),
                    FullyCompliantCount = table.Column<int>(nullable: false),
                    PartiallyCompliantCount = table.Column<int>(nullable: false),
                    NonCompliantCount = table.Column<int>(nullable: false),
                    NotApplicableCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificationProposals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CertificationProposals_AuditProjects_AuditProjectId1",
                        column: x => x.AuditProjectId1,
                        principalTable: "AuditProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CertificationProposals_EntityGroups_EntityGroupId",
                        column: x => x.EntityGroupId,
                        principalTable: "EntityGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CertificationProposals_AuditProjectId1",
                table: "CertificationProposals",
                column: "AuditProjectId1");

            migrationBuilder.CreateIndex(
                name: "IX_CertificationProposals_EntityGroupId",
                table: "CertificationProposals",
                column: "EntityGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CertificationProposals");
        }
    }
}
