using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class remove_unwanted_column_in_Statetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetFiledDataType",
                table: "State");

            migrationBuilder.DropColumn(
                name: "TargetFiledName",
                table: "State");

            migrationBuilder.DropColumn(
                name: "TargetFiledUpdatedValue",
                table: "State");

            migrationBuilder.DropColumn(
                name: "TargetFiledValue",
                table: "State");

            migrationBuilder.AddColumn<int>(
                name: "TargetFiled",
                table: "State",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetFiled",
                table: "State");

            migrationBuilder.AddColumn<string>(
                name: "TargetFiledDataType",
                table: "State",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetFiledName",
                table: "State",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetFiledUpdatedValue",
                table: "State",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetFiledValue",
                table: "State",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
