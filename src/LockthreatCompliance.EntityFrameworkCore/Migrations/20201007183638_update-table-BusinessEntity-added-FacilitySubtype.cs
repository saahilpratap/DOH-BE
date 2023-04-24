using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updatetableBusinessEntityaddedFacilitySubtype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FacilitySubTypeId",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessEntities_FacilitySubTypeId",
                table: "BusinessEntities",
                column: "FacilitySubTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessEntities_FacilitySubTypes_FacilitySubTypeId",
                table: "BusinessEntities",
                column: "FacilitySubTypeId",
                principalTable: "FacilitySubTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessEntities_FacilitySubTypes_FacilitySubTypeId",
                table: "BusinessEntities");

            migrationBuilder.DropIndex(
                name: "IX_BusinessEntities_FacilitySubTypeId",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "FacilitySubTypeId",
                table: "BusinessEntities");
        }
    }
}
