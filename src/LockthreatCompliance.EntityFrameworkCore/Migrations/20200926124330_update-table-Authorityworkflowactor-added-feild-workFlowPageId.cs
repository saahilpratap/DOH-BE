using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updatetableAuthorityworkflowactoraddedfeildworkFlowPageId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "WorkFlowNameId",
                table: "Authorityworkflowactors",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authorityworkflowactors_WorkFlowNameId",
                table: "Authorityworkflowactors",
                column: "WorkFlowNameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authorityworkflowactors_WorkFlowPage_WorkFlowNameId",
                table: "Authorityworkflowactors",
                column: "WorkFlowNameId",
                principalTable: "WorkFlowPage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authorityworkflowactors_WorkFlowPage_WorkFlowNameId",
                table: "Authorityworkflowactors");

            migrationBuilder.DropIndex(
                name: "IX_Authorityworkflowactors_WorkFlowNameId",
                table: "Authorityworkflowactors");

            migrationBuilder.DropColumn(
                name: "WorkFlowNameId",
                table: "Authorityworkflowactors");
        }
    }
}
