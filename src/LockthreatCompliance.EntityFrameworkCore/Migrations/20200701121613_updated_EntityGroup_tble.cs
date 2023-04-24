using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updated_EntityGroup_tble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PreAssessmentQuestionnaireAnsweredByGroupAdminOnly",
                table: "EntityGroups",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreAssessmentQuestionnaireAnsweredByGroupAdminOnly",
                table: "EntityGroups");
        }
    }
}
