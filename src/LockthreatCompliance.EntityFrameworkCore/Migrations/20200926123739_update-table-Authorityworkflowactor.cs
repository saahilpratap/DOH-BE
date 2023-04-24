using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updatetableAuthorityworkflowactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authorityworkflowactors_AbpDynamicParameterValues_WorkFlowNameId",
                table: "Authorityworkflowactors");

            migrationBuilder.DropIndex(
                name: "IX_Authorityworkflowactors_WorkFlowNameId",
                table: "Authorityworkflowactors");

            migrationBuilder.DropColumn(
                name: "WorkFlowNameId",
                table: "Authorityworkflowactors");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkFlowNameId",
                table: "Authorityworkflowactors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authorityworkflowactors_WorkFlowNameId",
                table: "Authorityworkflowactors",
                column: "WorkFlowNameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authorityworkflowactors_AbpDynamicParameterValues_WorkFlowNameId",
                table: "Authorityworkflowactors",
                column: "WorkFlowNameId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
