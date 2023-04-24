using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updatetableBusinessEntityremoveFacilitySubtypeDynamicparameter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessEntities_AbpDynamicParameterValues_FacilitySubTypeId",
                table: "BusinessEntities");

            migrationBuilder.DropIndex(
                name: "IX_BusinessEntities_FacilitySubTypeId",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "FacilitySubTypeId",
                table: "BusinessEntities");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FacilitySubTypeId",
                table: "BusinessEntities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessEntities_FacilitySubTypeId",
                table: "BusinessEntities",
                column: "FacilitySubTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessEntities_AbpDynamicParameterValues_FacilitySubTypeId",
                table: "BusinessEntities",
                column: "FacilitySubTypeId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
