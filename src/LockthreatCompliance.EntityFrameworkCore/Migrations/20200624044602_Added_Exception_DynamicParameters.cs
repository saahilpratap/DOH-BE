using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class Added_Exception_DynamicParameters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CriticalityId",
                table: "Exceptions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReviewPriorityId",
                table: "Exceptions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exceptions_CriticalityId",
                table: "Exceptions",
                column: "CriticalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Exceptions_ReviewPriorityId",
                table: "Exceptions",
                column: "ReviewPriorityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exceptions_AbpDynamicParameterValues_CriticalityId",
                table: "Exceptions",
                column: "CriticalityId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Exceptions_AbpDynamicParameterValues_ReviewPriorityId",
                table: "Exceptions",
                column: "ReviewPriorityId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exceptions_AbpDynamicParameterValues_CriticalityId",
                table: "Exceptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Exceptions_AbpDynamicParameterValues_ReviewPriorityId",
                table: "Exceptions");

            migrationBuilder.DropIndex(
                name: "IX_Exceptions_CriticalityId",
                table: "Exceptions");

            migrationBuilder.DropIndex(
                name: "IX_Exceptions_ReviewPriorityId",
                table: "Exceptions");

            migrationBuilder.DropColumn(
                name: "CriticalityId",
                table: "Exceptions");

            migrationBuilder.DropColumn(
                name: "ReviewPriorityId",
                table: "Exceptions");
        }
    }
}
