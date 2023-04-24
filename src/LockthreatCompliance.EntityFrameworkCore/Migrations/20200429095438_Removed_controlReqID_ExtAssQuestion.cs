using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class Removed_controlReqID_ExtAssQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalAssementQuestions_ControlRequirements_ControlRequirementId",
                table: "ExternalAssementQuestions");

            migrationBuilder.DropIndex(
                name: "IX_ExternalAssementQuestions_ControlRequirementId",
                table: "ExternalAssementQuestions");

            migrationBuilder.DropColumn(
                name: "ControlRequirementId",
                table: "ExternalAssementQuestions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ControlRequirementId",
                table: "ExternalAssementQuestions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssementQuestions_ControlRequirementId",
                table: "ExternalAssementQuestions",
                column: "ControlRequirementId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalAssementQuestions_ControlRequirements_ControlRequirementId",
                table: "ExternalAssementQuestions",
                column: "ControlRequirementId",
                principalTable: "ControlRequirements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
