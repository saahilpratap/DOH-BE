using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_ExternalAssessmentStatusLog_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalAssessmentStatusLogs",
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
                    AssessmentId = table.Column<int>(nullable: false),
                    ExternalAssessmentId = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UserActedId = table.Column<long>(nullable: true),
                    ActionDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalAssessmentStatusLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalAssessmentStatusLogs_ExternalAssessments_ExternalAssessmentId",
                        column: x => x.ExternalAssessmentId,
                        principalTable: "ExternalAssessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalAssessmentStatusLogs_AbpUsers_UserActedId",
                        column: x => x.UserActedId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessmentStatusLogs_ExternalAssessmentId",
                table: "ExternalAssessmentStatusLogs",
                column: "ExternalAssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessmentStatusLogs_UserActedId",
                table: "ExternalAssessmentStatusLogs",
                column: "UserActedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalAssessmentStatusLogs");
        }
    }
}
