using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class Remove_Errror_Tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalAssessments_ExternalAssessmentScheduleDetails_ExternalAssessmentScheduleDetailId",
                table: "ExternalAssessments");

            migrationBuilder.DropTable(
                name: "ExternalAssessmentScheduleDetails");

            migrationBuilder.DropTable(
                name: "ExternalAssessmentSchedules");

            migrationBuilder.DropIndex(
                name: "IX_ExternalAssessments_ExternalAssessmentScheduleDetailId",
                table: "ExternalAssessments");

            migrationBuilder.DropColumn(
                name: "ExternalAssessmentScheduleDetailId",
                table: "ExternalAssessments");

            migrationBuilder.DropColumn(
                name: "ScheduleDetailId",
                table: "ExternalAssessments");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ExternalAssessmentScheduleDetailId",
                table: "ExternalAssessments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ScheduleDetailId",
                table: "ExternalAssessments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExternalAssessmentSchedules",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentInfo = table.Column<string>(type: "nvarchar(max)", maxLength: 9999, nullable: true),
                    AssessmentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssessmentTypeId = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FeedBack = table.Column<string>(type: "nvarchar(max)", maxLength: 9999, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    RecurringJobId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ScheduleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScheduleTypeId = table.Column<int>(type: "int", nullable: true),
                    SendEmailNotify = table.Column<bool>(type: "bit", nullable: false),
                    SendSmsNotify = table.Column<bool>(type: "bit", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
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
                        name: "FK_ExternalAssessmentSchedules_AbpDynamicParameterValues_ScheduleTypeId",
                        column: x => x.ScheduleTypeId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });



            migrationBuilder.CreateTable(
                name: "ExternalAssessmentScheduleDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentInfo = table.Column<string>(type: "nvarchar(max)", maxLength: 9999, nullable: true),
                    AssessmentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssessmentTypeId = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExternalAssessmentScheduleId = table.Column<long>(type: "bigint", nullable: true),
                    FeedBack = table.Column<string>(type: "nvarchar(max)", maxLength: 9999, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    RecurringJobId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ScheduleId = table.Column<long>(type: "bigint", nullable: true),
                    ScheduleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScheduleTypeId = table.Column<int>(type: "int", nullable: true),
                    SendEmailNotify = table.Column<bool>(type: "bit", nullable: false),
                    SendSmsNotify = table.Column<bool>(type: "bit", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessments_ExternalAssessmentScheduleDetailId",
                table: "ExternalAssessments",
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

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessmentSchedules_AssessmentTypeId",
                table: "ExternalAssessmentSchedules",
                column: "AssessmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessmentSchedules_ScheduleTypeId",
                table: "ExternalAssessmentSchedules",
                column: "ScheduleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalAssessments_ExternalAssessmentScheduleDetails_ExternalAssessmentScheduleDetailId",
                table: "ExternalAssessments",
                column: "ExternalAssessmentScheduleDetailId",
                principalTable: "ExternalAssessmentScheduleDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
