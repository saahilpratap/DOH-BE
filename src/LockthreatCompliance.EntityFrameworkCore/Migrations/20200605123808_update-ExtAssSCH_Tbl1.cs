using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updateExtAssSCH_Tbl1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalAssessmentScheduleDetails",
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
                    ScheduleId = table.Column<long>(nullable: true),
                    ExternalAssessmentScheduleId = table.Column<long>(nullable: true),
                    ScheduleName = table.Column<string>(nullable: true),
                    AssessmentName = table.Column<string>(nullable: true),
                    AssessmentInfo = table.Column<string>(maxLength: 9999, nullable: true),
                    AssessmentTypeId = table.Column<int>(nullable: true),
                    ScheduleTypeId = table.Column<int>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    FeedBack = table.Column<string>(maxLength: 9999, nullable: true),
                    SendEmailNotify = table.Column<bool>(nullable: false),
                    SendSmsNotify = table.Column<bool>(nullable: false),
                    RecurringJobId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalAssessmentScheduleDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalAssessmentScheduleDetails_AbpDynamicParameterValues_AssessmentTypeId",
                        column: x => x.AssessmentTypeId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalAssessmentScheduleDetails_ExternalAssessmentSchedules_ExternalAssessmentScheduleId",
                        column: x => x.ExternalAssessmentScheduleId,
                        principalTable: "ExternalAssessmentSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalAssessmentScheduleDetails_AbpDynamicParameterValues_ScheduleTypeId",
                        column: x => x.ScheduleTypeId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExtAssSchDetailAuthoritativeDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthoritativeDocumentId = table.Column<int>(nullable: false),
                    ExternalAssessmentScheduleDetailId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtAssSchDetailAuthoritativeDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExtAssSchDetailAuthoritativeDocuments_AuthoritativeDocuments_AuthoritativeDocumentId",
                        column: x => x.AuthoritativeDocumentId,
                        principalTable: "AuthoritativeDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExtAssSchDetailAuthoritativeDocuments_ExternalAssessmentScheduleDetails_ExternalAssessmentScheduleDetailId",
                        column: x => x.ExternalAssessmentScheduleDetailId,
                        principalTable: "ExternalAssessmentScheduleDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExtAssSchDetailAuthoritativeDocuments_AuthoritativeDocumentId",
                table: "ExtAssSchDetailAuthoritativeDocuments",
                column: "AuthoritativeDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtAssSchDetailAuthoritativeDocuments_ExternalAssessmentScheduleDetailId",
                table: "ExtAssSchDetailAuthoritativeDocuments",
                column: "ExternalAssessmentScheduleDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessmentScheduleDetails_AssessmentTypeId",
                table: "ExternalAssessmentScheduleDetails",
                column: "AssessmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessmentScheduleDetails_ExternalAssessmentScheduleId",
                table: "ExternalAssessmentScheduleDetails",
                column: "ExternalAssessmentScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessmentScheduleDetails_ScheduleTypeId",
                table: "ExternalAssessmentScheduleDetails",
                column: "ScheduleTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExtAssSchDetailAuthoritativeDocuments");

            migrationBuilder.DropTable(
                name: "ExternalAssessmentScheduleDetails");
        }
    }
}
