using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updatetableBusinessEntityWorkFlowActoraddedfeildworkFlowPageId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "WorkFlowNameId",
                table: "BusinessEntityWorkFlowActor",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessEntityWorkFlowActor_WorkFlowNameId",
                table: "BusinessEntityWorkFlowActor",
                column: "WorkFlowNameId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessEntityWorkFlowActor_WorkFlowPage_WorkFlowNameId",
                table: "BusinessEntityWorkFlowActor",
                column: "WorkFlowNameId",
                principalTable: "WorkFlowPage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessEntityWorkFlowActor_WorkFlowPage_WorkFlowNameId",
                table: "BusinessEntityWorkFlowActor");

            migrationBuilder.DropIndex(
                name: "IX_BusinessEntityWorkFlowActor_WorkFlowNameId",
                table: "BusinessEntityWorkFlowActor");

            migrationBuilder.DropColumn(
                name: "WorkFlowNameId",
                table: "BusinessEntityWorkFlowActor");
        }
    }
}
