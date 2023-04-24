using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedTableAssessmentSubmissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssessmentSubmissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessEntityId = table.Column<int>(nullable: true),
                    AssessmentId = table.Column<int>(nullable: false),
                    NotificationText = table.Column<string>(nullable: true),
                    SubmissionDate = table.Column<DateTime>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentSubmissions_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssessmentSubmissions_BusinessEntities_BusinessEntityId",
                        column: x => x.BusinessEntityId,
                        principalTable: "BusinessEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentSubmissions_AssessmentId",
                table: "AssessmentSubmissions",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentSubmissions_BusinessEntityId",
                table: "AssessmentSubmissions",
                column: "BusinessEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssessmentSubmissions");
        }
    }
}
