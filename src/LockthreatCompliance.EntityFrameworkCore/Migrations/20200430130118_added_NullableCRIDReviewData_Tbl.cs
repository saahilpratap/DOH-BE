using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_NullableCRIDReviewData_Tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewDatas_ControlRequirements_ControlRequirementId",
                table: "ReviewDatas");

            migrationBuilder.AlterColumn<int>(
                name: "ControlRequirementId",
                table: "ReviewDatas",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewDatas_ControlRequirements_ControlRequirementId",
                table: "ReviewDatas",
                column: "ControlRequirementId",
                principalTable: "ControlRequirements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewDatas_ControlRequirements_ControlRequirementId",
                table: "ReviewDatas");

            migrationBuilder.AlterColumn<int>(
                name: "ControlRequirementId",
                table: "ReviewDatas",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewDatas_ControlRequirements_ControlRequirementId",
                table: "ReviewDatas",
                column: "ControlRequirementId",
                principalTable: "ControlRequirements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
