using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class Added_Field_Attachment_In_QuestionResponse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Attachment",
                table: "QuestResponses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "QuestResponses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attachment",
                table: "QuestResponses");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "QuestResponses");
        }
    }
}
