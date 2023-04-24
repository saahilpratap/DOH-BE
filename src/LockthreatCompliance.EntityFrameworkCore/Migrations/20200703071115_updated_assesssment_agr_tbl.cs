using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updated_assesssment_agr_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssessmentAgreementResponses_Assessments_AssessmentId",
                table: "AssessmentAgreementResponses");

            migrationBuilder.AlterColumn<int>(
                name: "AssessmentId",
                table: "AssessmentAgreementResponses",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ExternalAssessmentId",
                table: "AssessmentAgreementResponses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentAgreementResponses_ExternalAssessmentId",
                table: "AssessmentAgreementResponses",
                column: "ExternalAssessmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssessmentAgreementResponses_Assessments_AssessmentId",
                table: "AssessmentAgreementResponses",
                column: "AssessmentId",
                principalTable: "Assessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssessmentAgreementResponses_ExternalAssessments_ExternalAssessmentId",
                table: "AssessmentAgreementResponses",
                column: "ExternalAssessmentId",
                principalTable: "ExternalAssessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssessmentAgreementResponses_Assessments_AssessmentId",
                table: "AssessmentAgreementResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_AssessmentAgreementResponses_ExternalAssessments_ExternalAssessmentId",
                table: "AssessmentAgreementResponses");

            migrationBuilder.DropIndex(
                name: "IX_AssessmentAgreementResponses_ExternalAssessmentId",
                table: "AssessmentAgreementResponses");

            migrationBuilder.DropColumn(
                name: "ExternalAssessmentId",
                table: "AssessmentAgreementResponses");

            migrationBuilder.AlterColumn<int>(
                name: "AssessmentId",
                table: "AssessmentAgreementResponses",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AssessmentAgreementResponses_Assessments_AssessmentId",
                table: "AssessmentAgreementResponses",
                column: "AssessmentId",
                principalTable: "Assessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
