using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_AssessmentStatusLog_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssessmentStatusLogs",
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
                    Status = table.Column<int>(nullable: false),
                    UserActedId = table.Column<long>(nullable: true),
                    ActionDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentStatusLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentStatusLogs_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssessmentStatusLogs_AbpUsers_UserActedId",
                        column: x => x.UserActedId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentStatusLogs_AssessmentId",
                table: "AssessmentStatusLogs",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentStatusLogs_UserActedId",
                table: "AssessmentStatusLogs",
                column: "UserActedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssessmentStatusLogs");
        }
    }
}
