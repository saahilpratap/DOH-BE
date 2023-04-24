using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updated_DynamicParameter_in_BusinessEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessEntities_AbpDynamicParameterValues_ThirdId",
                table: "BusinessEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_PreRegisterBusinessEntities_AbpDynamicParameterValues_ThirdId",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropIndex(
                name: "IX_PreRegisterBusinessEntities_ThirdId",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropIndex(
                name: "IX_BusinessEntities_ThirdId",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "ThirdId",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "ThirdId",
                table: "BusinessEntities");

            migrationBuilder.CreateIndex(
                name: "IX_PreRegisterBusinessEntities_ThirdPartyId",
                table: "PreRegisterBusinessEntities",
                column: "ThirdPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessEntities_ThirdPartyId",
                table: "BusinessEntities",
                column: "ThirdPartyId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessEntities_AbpDynamicParameterValues_ThirdPartyId",
                table: "BusinessEntities",
                column: "ThirdPartyId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PreRegisterBusinessEntities_AbpDynamicParameterValues_ThirdPartyId",
                table: "PreRegisterBusinessEntities",
                column: "ThirdPartyId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessEntities_AbpDynamicParameterValues_ThirdPartyId",
                table: "BusinessEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_PreRegisterBusinessEntities_AbpDynamicParameterValues_ThirdPartyId",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropIndex(
                name: "IX_PreRegisterBusinessEntities_ThirdPartyId",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropIndex(
                name: "IX_BusinessEntities_ThirdPartyId",
                table: "BusinessEntities");

            migrationBuilder.AddColumn<int>(
                name: "ThirdId",
                table: "PreRegisterBusinessEntities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThirdId",
                table: "BusinessEntities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PreRegisterBusinessEntities_ThirdId",
                table: "PreRegisterBusinessEntities",
                column: "ThirdId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessEntities_ThirdId",
                table: "BusinessEntities",
                column: "ThirdId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessEntities_AbpDynamicParameterValues_ThirdId",
                table: "BusinessEntities",
                column: "ThirdId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PreRegisterBusinessEntities_AbpDynamicParameterValues_ThirdId",
                table: "PreRegisterBusinessEntities",
                column: "ThirdId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
