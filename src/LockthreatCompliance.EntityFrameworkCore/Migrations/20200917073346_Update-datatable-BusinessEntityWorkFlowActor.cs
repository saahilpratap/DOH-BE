using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class UpdatedatatableBusinessEntityWorkFlowActor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthoritativeDocumentId",
                table: "BusinessEntityWorkFlowActor",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkFlowNameId",
                table: "BusinessEntityWorkFlowActor",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessEntityWorkFlowActor_AuthoritativeDocumentId",
                table: "BusinessEntityWorkFlowActor",
                column: "AuthoritativeDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessEntityWorkFlowActor_WorkFlowNameId",
                table: "BusinessEntityWorkFlowActor",
                column: "WorkFlowNameId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessEntityWorkFlowActor_AuthoritativeDocuments_AuthoritativeDocumentId",
                table: "BusinessEntityWorkFlowActor",
                column: "AuthoritativeDocumentId",
                principalTable: "AuthoritativeDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessEntityWorkFlowActor_AbpDynamicParameterValues_WorkFlowNameId",
                table: "BusinessEntityWorkFlowActor",
                column: "WorkFlowNameId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessEntityWorkFlowActor_AuthoritativeDocuments_AuthoritativeDocumentId",
                table: "BusinessEntityWorkFlowActor");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessEntityWorkFlowActor_AbpDynamicParameterValues_WorkFlowNameId",
                table: "BusinessEntityWorkFlowActor");

            migrationBuilder.DropIndex(
                name: "IX_BusinessEntityWorkFlowActor_AuthoritativeDocumentId",
                table: "BusinessEntityWorkFlowActor");

            migrationBuilder.DropIndex(
                name: "IX_BusinessEntityWorkFlowActor_WorkFlowNameId",
                table: "BusinessEntityWorkFlowActor");

            migrationBuilder.DropColumn(
                name: "AuthoritativeDocumentId",
                table: "BusinessEntityWorkFlowActor");

            migrationBuilder.DropColumn(
                name: "WorkFlowNameId",
                table: "BusinessEntityWorkFlowActor");
        }
    }
}
