using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class UpdatetableAssesmentColumnaddedEntityGroupId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EntityGroupId",
                table: "Assessments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_EntityGroupId",
                table: "Assessments",
                column: "EntityGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_EntityGroups_EntityGroupId",
                table: "Assessments",
                column: "EntityGroupId",
                principalTable: "EntityGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_EntityGroups_EntityGroupId",
                table: "Assessments");

            migrationBuilder.DropIndex(
                name: "IX_Assessments_EntityGroupId",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "EntityGroupId",
                table: "Assessments");
        }
    }
}
