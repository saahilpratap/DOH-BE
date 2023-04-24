using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class Add_ExternalAssessmentSchedules_Tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalAssessmentSchedules",
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
                    ScheduleName = table.Column<string>(nullable: true),
                    AssessmentName = table.Column<string>(nullable: true),
                    AssessmentInfo = table.Column<string>(maxLength: 9999, nullable: true),
                    AssessmentTypeId = table.Column<int>(nullable: true),
                    ScheduleTypeId = table.Column<int>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    AuthoritativeDocumentId = table.Column<int>(nullable: false),
                    FeedBack = table.Column<string>(maxLength: 9999, nullable: true),
                    SendEmailNotify = table.Column<bool>(nullable: false),
                    SendSmsNotify = table.Column<bool>(nullable: false),
                    RecurringJobId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalAssessmentSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalAssessmentSchedules_AbpDynamicParameterValues_AssessmentTypeId",
                        column: x => x.AssessmentTypeId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalAssessmentSchedules_AuthoritativeDocuments_AuthoritativeDocumentId",
                        column: x => x.AuthoritativeDocumentId,
                        principalTable: "AuthoritativeDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExternalAssessmentSchedules_AbpDynamicParameterValues_ScheduleTypeId",
                        column: x => x.ScheduleTypeId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessmentSchedules_AssessmentTypeId",
                table: "ExternalAssessmentSchedules",
                column: "AssessmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessmentSchedules_AuthoritativeDocumentId",
                table: "ExternalAssessmentSchedules",
                column: "AuthoritativeDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessmentSchedules_ScheduleTypeId",
                table: "ExternalAssessmentSchedules",
                column: "ScheduleTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalAssessmentSchedules");
        }
    }
}
