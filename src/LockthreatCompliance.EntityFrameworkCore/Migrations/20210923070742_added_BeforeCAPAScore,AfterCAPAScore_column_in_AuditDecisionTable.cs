using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_BeforeCAPAScoreAfterCAPAScore_column_in_AuditDecisionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AfterCAPAScore",
                table: "AuditDecForms",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BeforeCAPAScore",
                table: "AuditDecForms",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AfterCAPAScore",
                table: "AuditDecForms");

            migrationBuilder.DropColumn(
                name: "BeforeCAPAScore",
                table: "AuditDecForms");
        }
    }
}
