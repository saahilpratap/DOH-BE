using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class UpdatetableQuestionGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DomainTitle",
                table: "QuestionGroups",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SectionTitle",
                table: "QuestionGroups",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubDomainTitle",
                table: "QuestionGroups",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DomainTitle",
                table: "QuestionGroups");

            migrationBuilder.DropColumn(
                name: "SectionTitle",
                table: "QuestionGroups");

            migrationBuilder.DropColumn(
                name: "SubDomainTitle",
                table: "QuestionGroups");
        }
    }
}
