using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updatetablePreRegisterBusinessEntityaddedfacilitySubtype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FacilitySubTypeId",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PreRegisterBusinessEntities_FacilitySubTypeId",
                table: "PreRegisterBusinessEntities",
                column: "FacilitySubTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreRegisterBusinessEntities_FacilitySubTypes_FacilitySubTypeId",
                table: "PreRegisterBusinessEntities",
                column: "FacilitySubTypeId",
                principalTable: "FacilitySubTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreRegisterBusinessEntities_FacilitySubTypes_FacilitySubTypeId",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropIndex(
                name: "IX_PreRegisterBusinessEntities_FacilitySubTypeId",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "FacilitySubTypeId",
                table: "PreRegisterBusinessEntities");
        }
    }
}
