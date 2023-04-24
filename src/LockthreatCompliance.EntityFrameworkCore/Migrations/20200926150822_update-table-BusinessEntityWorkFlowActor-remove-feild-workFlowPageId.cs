using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updatetableBusinessEntityWorkFlowActorremovefeildworkFlowPageId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessEntityWorkFlowActor_AbpDynamicParameterValues_WorkFlowNameId",
                table: "BusinessEntityWorkFlowActor");

            migrationBuilder.DropIndex(
                name: "IX_BusinessEntityWorkFlowActor_WorkFlowNameId",
                table: "BusinessEntityWorkFlowActor");

            migrationBuilder.DropColumn(
                name: "WorkFlowNameId",
                table: "BusinessEntityWorkFlowActor");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkFlowNameId",
                table: "BusinessEntityWorkFlowActor",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessEntityWorkFlowActor_WorkFlowNameId",
                table: "BusinessEntityWorkFlowActor",
                column: "WorkFlowNameId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessEntityWorkFlowActor_AbpDynamicParameterValues_WorkFlowNameId",
                table: "BusinessEntityWorkFlowActor",
                column: "WorkFlowNameId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
