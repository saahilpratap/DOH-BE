using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updatetablePreRegisterBusinessEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CityOrDisctrict",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "PreRegisterBusinessEntities",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PreRegisterBusinessEntities_CountryId",
                table: "PreRegisterBusinessEntities",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreRegisterBusinessEntities_Countries_CountryId",
                table: "PreRegisterBusinessEntities",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreRegisterBusinessEntities_Countries_CountryId",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropIndex(
                name: "IX_PreRegisterBusinessEntities_CountryId",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "CityOrDisctrict",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "PreRegisterBusinessEntities");
        }
    }
}
