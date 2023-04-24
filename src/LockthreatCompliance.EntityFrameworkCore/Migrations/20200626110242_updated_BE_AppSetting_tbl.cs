using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updated_BE_AppSetting_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LoginScreenDisclaimerMesg",
                table: "EntityApplicationSettings",
                maxLength: 9999999,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(999)",
                oldMaxLength: 999,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AgreementAcceptanceMsg",
                table: "EntityApplicationSettings",
                maxLength: 999999999,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPreAssessmentQuestionaire",
                table: "EntityApplicationSettings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CISO_EN",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CISO_Email",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CISO_Mobile",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPreAssessmentQuestionaire",
                table: "BusinessEntities",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgreementAcceptanceMsg",
                table: "EntityApplicationSettings");

            migrationBuilder.DropColumn(
                name: "IsPreAssessmentQuestionaire",
                table: "EntityApplicationSettings");

            migrationBuilder.DropColumn(
                name: "CISO_EN",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "CISO_Email",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "CISO_Mobile",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "IsPreAssessmentQuestionaire",
                table: "BusinessEntities");

            migrationBuilder.AlterColumn<string>(
                name: "LoginScreenDisclaimerMesg",
                table: "EntityApplicationSettings",
                type: "nvarchar(999)",
                maxLength: 999,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 9999999,
                oldNullable: true);
        }
    }
}
