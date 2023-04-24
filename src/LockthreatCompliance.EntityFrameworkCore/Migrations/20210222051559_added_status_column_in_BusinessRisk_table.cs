using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_status_column_in_BusinessRisk_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "BusinessRisks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRisks_StatusId",
                table: "BusinessRisks",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessRisks_AbpDynamicParameterValues_StatusId",
                table: "BusinessRisks",
                column: "StatusId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessRisks_AbpDynamicParameterValues_StatusId",
                table: "BusinessRisks");

            migrationBuilder.DropIndex(
                name: "IX_BusinessRisks_StatusId",
                table: "BusinessRisks");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "BusinessRisks");
        }
    }
}
