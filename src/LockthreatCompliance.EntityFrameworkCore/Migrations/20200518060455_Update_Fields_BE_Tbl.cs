using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class Update_Fields_BE_Tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Director_Incharge_EN",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Director_Incharge_Email",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Director_Incharge_Mobile",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DistrictId",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FacilitySubTypeId",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Facility_EN",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Facility_Email",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HFLName",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "BusinessEntities",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "BusinessEntities",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Owner_EN",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Owner_Email",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Owner_Mobile",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pro_EN",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pro_Email",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pro_Mobile",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessEntities_DistrictId",
                table: "BusinessEntities",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessEntities_FacilitySubTypeId",
                table: "BusinessEntities",
                column: "FacilitySubTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessEntities_AbpDynamicParameterValues_DistrictId",
                table: "BusinessEntities",
                column: "DistrictId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessEntities_AbpDynamicParameterValues_FacilitySubTypeId",
                table: "BusinessEntities",
                column: "FacilitySubTypeId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessEntities_AbpDynamicParameterValues_DistrictId",
                table: "BusinessEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessEntities_AbpDynamicParameterValues_FacilitySubTypeId",
                table: "BusinessEntities");

            migrationBuilder.DropIndex(
                name: "IX_BusinessEntities_DistrictId",
                table: "BusinessEntities");

            migrationBuilder.DropIndex(
                name: "IX_BusinessEntities_FacilitySubTypeId",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "Director_Incharge_EN",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "Director_Incharge_Email",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "Director_Incharge_Mobile",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "FacilitySubTypeId",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "Facility_EN",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "Facility_Email",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "HFLName",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "Owner_EN",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "Owner_Email",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "Owner_Mobile",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "Pro_EN",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "Pro_Email",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "Pro_Mobile",
                table: "BusinessEntities");
        }
    }
}
