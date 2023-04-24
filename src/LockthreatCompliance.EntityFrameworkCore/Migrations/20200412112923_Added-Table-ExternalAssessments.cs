using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedTableExternalAssessments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalAssessments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    FiscalYear = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    BusinessEntityId = table.Column<int>(nullable: false),
                    LeadAssessorId = table.Column<long>(nullable: true),
                    HasQuestionaireGenerated = table.Column<bool>(nullable: false),
                    VendorId = table.Column<int>(nullable: true),
                    BusinessEntityLeadAssessorId = table.Column<long>(nullable: true),
                    CreatorUserId = table.Column<long>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalAssessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalAssessments_BusinessEntities_BusinessEntityId",
                        column: x => x.BusinessEntityId,
                        principalTable: "BusinessEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExternalAssessments_AbpUsers_BusinessEntityLeadAssessorId",
                        column: x => x.BusinessEntityLeadAssessorId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalAssessments_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalAssessments_AbpUsers_LeadAssessorId",
                        column: x => x.LeadAssessorId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalAssessments_BusinessEntities_VendorId",
                        column: x => x.VendorId,
                        principalTable: "BusinessEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExternalAssessmentAuthoritativeDocument",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthoritativeDocumentId = table.Column<int>(nullable: false),
                    ExternalAssessmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalAssessmentAuthoritativeDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalAssessmentAuthoritativeDocument_AuthoritativeDocuments_AuthoritativeDocumentId",
                        column: x => x.AuthoritativeDocumentId,
                        principalTable: "AuthoritativeDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExternalAssessmentAuthoritativeDocument_ExternalAssessments_ExternalAssessmentId",
                        column: x => x.ExternalAssessmentId,
                        principalTable: "ExternalAssessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessmentAuthoritativeDocument_AuthoritativeDocumentId",
                table: "ExternalAssessmentAuthoritativeDocument",
                column: "AuthoritativeDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessmentAuthoritativeDocument_ExternalAssessmentId",
                table: "ExternalAssessmentAuthoritativeDocument",
                column: "ExternalAssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessments_BusinessEntityId",
                table: "ExternalAssessments",
                column: "BusinessEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessments_BusinessEntityLeadAssessorId",
                table: "ExternalAssessments",
                column: "BusinessEntityLeadAssessorId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessments_CreatorUserId",
                table: "ExternalAssessments",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessments_LeadAssessorId",
                table: "ExternalAssessments",
                column: "LeadAssessorId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessments_VendorId",
                table: "ExternalAssessments",
                column: "VendorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalAssessmentAuthoritativeDocument");

            migrationBuilder.DropTable(
                name: "ExternalAssessments");
        }
    }
}
