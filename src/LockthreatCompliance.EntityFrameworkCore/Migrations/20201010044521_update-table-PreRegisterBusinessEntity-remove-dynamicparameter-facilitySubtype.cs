using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updatetablePreRegisterBusinessEntityremovedynamicparameterfacilitySubtype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreRegisterBusinessEntities_AbpDynamicParameterValues_FacilitySubTypeId",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropIndex(
                name: "IX_PreRegisterBusinessEntities_FacilitySubTypeId",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "FacilitySubTypeId",
                table: "PreRegisterBusinessEntities");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FacilitySubTypeId",
                table: "PreRegisterBusinessEntities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PreRegisterBusinessEntities_FacilitySubTypeId",
                table: "PreRegisterBusinessEntities",
                column: "FacilitySubTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreRegisterBusinessEntities_AbpDynamicParameterValues_FacilitySubTypeId",
                table: "PreRegisterBusinessEntities",
                column: "FacilitySubTypeId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
