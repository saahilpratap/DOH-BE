using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class UpdatetabelExternalAssementsEntityGroupId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EntityGroupId",
                table: "ExternalAssessments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAssessments_EntityGroupId",
                table: "ExternalAssessments",
                column: "EntityGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalAssessments_EntityGroups_EntityGroupId",
                table: "ExternalAssessments",
                column: "EntityGroupId",
                principalTable: "EntityGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalAssessments_EntityGroups_EntityGroupId",
                table: "ExternalAssessments");

            migrationBuilder.DropIndex(
                name: "IX_ExternalAssessments_EntityGroupId",
                table: "ExternalAssessments");

            migrationBuilder.DropColumn(
                name: "EntityGroupId",
                table: "ExternalAssessments");
        }
    }
}
