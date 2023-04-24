using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class Added_EAWPTable_DocumentTble_Change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AWPId",
                table: "DocumentPaths",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AWPId1",
                table: "DocumentPaths",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExternalAssessmentAuditWorkPapers",
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
                    ExternalAssessmentId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    DatePrepared = table.Column<DateTime>(nullable: false),
                    AWPTypeId = table.Column<int>(nullable: true),
                    TypeId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Signature = table.Column<string>(maxLength: 9999, nullable: true),
                    Notes = table.Column<string>(maxLength: 99999, nullable: true),
                    Link = table.Column<string>(maxLength: 999, nullable: true),
                    MeetingAgenda = table.Column<string>(nullable: true),
                    MgmtChecklist = table.Column<string>(nullable: true),
                    GeneralAttachment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalAssessmentAuditWorkPapers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalAssessmentAuditWorkPapers_ExternalAssessments_ExternalAssessmentId",
                        column: x => x.ExternalAssessmentId,
                        principalTable: "ExternalAssessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExternalAssessmentAuditWorkPapers_AbpDynamicParameterValues_TypeId",
                        column: x => x.TypeId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPaths_AWPId1",
                table: "DocumentPaths",
                column: "AWPId1");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessmentAuditWorkPapers_ExternalAssessmentId",
                table: "ExternalAssessmentAuditWorkPapers",
                column: "ExternalAssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessmentAuditWorkPapers_TypeId",
                table: "ExternalAssessmentAuditWorkPapers",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentPaths_ExternalAssessmentAuditWorkPapers_AWPId1",
                table: "DocumentPaths",
                column: "AWPId1",
                principalTable: "ExternalAssessmentAuditWorkPapers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentPaths_ExternalAssessmentAuditWorkPapers_AWPId1",
                table: "DocumentPaths");

            migrationBuilder.DropTable(
                name: "ExternalAssessmentAuditWorkPapers");

            migrationBuilder.DropIndex(
                name: "IX_DocumentPaths_AWPId1",
                table: "DocumentPaths");

            migrationBuilder.DropColumn(
                name: "AWPId",
                table: "DocumentPaths");

            migrationBuilder.DropColumn(
                name: "AWPId1",
                table: "DocumentPaths");
        }
    }
}
