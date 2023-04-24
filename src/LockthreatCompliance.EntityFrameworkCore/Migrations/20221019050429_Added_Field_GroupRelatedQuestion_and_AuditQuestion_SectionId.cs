using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class Added_Field_GroupRelatedQuestion_and_AuditQuestion_SectionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SectionId",
                table: "GroupRelatedQuestions",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SectionId",
                table: "AuditQuestResponses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupRelatedQuestions_SectionId",
                table: "GroupRelatedQuestions",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditQuestResponses_SectionId",
                table: "AuditQuestResponses",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditQuestResponses_Sections_SectionId",
                table: "AuditQuestResponses",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupRelatedQuestions_Sections_SectionId",
                table: "GroupRelatedQuestions",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditQuestResponses_Sections_SectionId",
                table: "AuditQuestResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupRelatedQuestions_Sections_SectionId",
                table: "GroupRelatedQuestions");

            migrationBuilder.DropIndex(
                name: "IX_GroupRelatedQuestions_SectionId",
                table: "GroupRelatedQuestions");

            migrationBuilder.DropIndex(
                name: "IX_AuditQuestResponses_SectionId",
                table: "AuditQuestResponses");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "GroupRelatedQuestions");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "AuditQuestResponses");
        }
    }
}
