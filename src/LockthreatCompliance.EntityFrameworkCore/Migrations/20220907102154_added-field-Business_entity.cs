using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class addedfieldBusiness_entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConnectivityId",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ScannerConnectivity",
                table: "BusinessEntities",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessEntities_ConnectivityId",
                table: "BusinessEntities",
                column: "ConnectivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessEntities_AbpDynamicParameterValues_ConnectivityId",
                table: "BusinessEntities",
                column: "ConnectivityId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessEntities_AbpDynamicParameterValues_ConnectivityId",
                table: "BusinessEntities");

            migrationBuilder.DropIndex(
                name: "IX_BusinessEntities_ConnectivityId",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "ConnectivityId",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "ScannerConnectivity",
                table: "BusinessEntities");
        }
    }
}
