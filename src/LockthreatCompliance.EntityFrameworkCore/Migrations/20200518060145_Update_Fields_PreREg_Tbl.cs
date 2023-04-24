using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class Update_Fields_PreREg_Tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Director_Incharge_EN",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Director_Incharge_Email",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Director_Incharge_Mobile",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DistrictId",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FacilitySubTypeId",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FacilityTypeId",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Facility_EN",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Facility_Email",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HFLName",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PreRegisterBusinessEntities",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "PreRegisterBusinessEntities",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Owner_EN",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Owner_Email",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Owner_Mobile",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pro_EN",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pro_Email",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pro_Mobile",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PreRegisterBusinessEntities_DistrictId",
                table: "PreRegisterBusinessEntities",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_PreRegisterBusinessEntities_FacilitySubTypeId",
                table: "PreRegisterBusinessEntities",
                column: "FacilitySubTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PreRegisterBusinessEntities_FacilityTypeId",
                table: "PreRegisterBusinessEntities",
                column: "FacilityTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreRegisterBusinessEntities_AbpDynamicParameterValues_DistrictId",
                table: "PreRegisterBusinessEntities",
                column: "DistrictId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PreRegisterBusinessEntities_AbpDynamicParameterValues_FacilitySubTypeId",
                table: "PreRegisterBusinessEntities",
                column: "FacilitySubTypeId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PreRegisterBusinessEntities_FacilityTypes_FacilityTypeId",
                table: "PreRegisterBusinessEntities",
                column: "FacilityTypeId",
                principalTable: "FacilityTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreRegisterBusinessEntities_AbpDynamicParameterValues_DistrictId",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_PreRegisterBusinessEntities_AbpDynamicParameterValues_FacilitySubTypeId",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_PreRegisterBusinessEntities_FacilityTypes_FacilityTypeId",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropIndex(
                name: "IX_PreRegisterBusinessEntities_DistrictId",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropIndex(
                name: "IX_PreRegisterBusinessEntities_FacilitySubTypeId",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropIndex(
                name: "IX_PreRegisterBusinessEntities_FacilityTypeId",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "Director_Incharge_EN",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "Director_Incharge_Email",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "Director_Incharge_Mobile",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "FacilitySubTypeId",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "FacilityTypeId",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "Facility_EN",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "Facility_Email",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "HFLName",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "Owner_EN",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "Owner_Email",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "Owner_Mobile",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "Pro_EN",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "Pro_Email",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "Pro_Mobile",
                table: "PreRegisterBusinessEntities");
        }
    }
}
