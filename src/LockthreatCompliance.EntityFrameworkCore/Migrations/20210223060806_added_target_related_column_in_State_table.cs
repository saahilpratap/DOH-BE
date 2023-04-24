using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_target_related_column_in_State_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TargetFiledDataType",
                table: "State",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetFiledName",
                table: "State",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetFiledValue",
                table: "State",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetFiledDataType",
                table: "State");

            migrationBuilder.DropColumn(
                name: "TargetFiledName",
                table: "State");

            migrationBuilder.DropColumn(
                name: "TargetFiledValue",
                table: "State");
        }
    }
}
