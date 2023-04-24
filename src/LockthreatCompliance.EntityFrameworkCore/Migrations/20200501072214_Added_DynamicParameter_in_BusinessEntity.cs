using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class Added_DynamicParameter_in_BusinessEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ThirdId",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThirdPartyId",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThirdId",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThirdPartyId",
                table: "BusinessEntities",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "ThirdPartyId",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "ThirdId",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "ThirdPartyId",
                table: "BusinessEntities");
        }
    }
}
