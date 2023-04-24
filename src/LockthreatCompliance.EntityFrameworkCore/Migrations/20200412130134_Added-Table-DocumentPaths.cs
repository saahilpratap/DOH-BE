using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedTableDocumentPaths : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentPaths",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    ReviewDataId = table.Column<int>(nullable: true),
                    ReviewQuestionId = table.Column<int>(nullable: true),
                    FindingReportId = table.Column<int>(nullable: true),
                    BusinessRiskId = table.Column<int>(nullable: true),
                    IncidentId = table.Column<int>(nullable: true),
                    ExceptionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentPaths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentPaths_BusinessRisks_BusinessRiskId",
                        column: x => x.BusinessRiskId,
                        principalTable: "BusinessRisks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentPaths_Exceptions_ExceptionId",
                        column: x => x.ExceptionId,
                        principalTable: "Exceptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentPaths_FindingReports_FindingReportId",
                        column: x => x.FindingReportId,
                        principalTable: "FindingReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentPaths_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentPaths_ReviewDatas_ReviewDataId",
                        column: x => x.ReviewDataId,
                        principalTable: "ReviewDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentPaths_ReviewQuestions_ReviewQuestionId",
                        column: x => x.ReviewQuestionId,
                        principalTable: "ReviewQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPaths_BusinessRiskId",
                table: "DocumentPaths",
                column: "BusinessRiskId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPaths_ExceptionId",
                table: "DocumentPaths",
                column: "ExceptionId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPaths_FindingReportId",
                table: "DocumentPaths",
                column: "FindingReportId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPaths_IncidentId",
                table: "DocumentPaths",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPaths_ReviewDataId",
                table: "DocumentPaths",
                column: "ReviewDataId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPaths_ReviewQuestionId",
                table: "DocumentPaths",
                column: "ReviewQuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentPaths");
        }
    }
}
