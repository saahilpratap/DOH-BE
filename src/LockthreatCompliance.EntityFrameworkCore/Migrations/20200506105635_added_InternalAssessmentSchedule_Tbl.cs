using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_InternalAssessmentSchedule_Tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InternalAssessmentSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    ScheduleName = table.Column<string>(nullable: true),
                    AssessmentName = table.Column<string>(nullable: true),
                    AssessmentInfo = table.Column<string>(maxLength: 9999, nullable: true),
                    AssessmentType = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_InternalAssessmentSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InternalAssessmentSchedules_AuthoritativeDocuments_AuthoritativeDocumentId",
                        column: x => x.AuthoritativeDocumentId,
                        principalTable: "AuthoritativeDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InternalAssessmentSchedules_AbpDynamicParameterValues_ScheduleTypeId",
                        column: x => x.ScheduleTypeId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InternalAssessmentSchedules_AuthoritativeDocumentId",
                table: "InternalAssessmentSchedules",
                column: "AuthoritativeDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalAssessmentSchedules_ScheduleTypeId",
                table: "InternalAssessmentSchedules",
                column: "ScheduleTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InternalAssessmentSchedules");
        }
    }
}
