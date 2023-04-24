using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updated_BusinessEntity_tble_Colmns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessEntityWorkFlowActor_AbpUsers_UserId",
                table: "BusinessEntityWorkFlowActor");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "BusinessEntityWorkFlowActor",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "NotifierId",
                table: "BusinessEntityWorkFlowActor",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NotifierUserId",
                table: "BusinessEntityWorkFlowActor",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessEntityWorkFlowActor_NotifierId",
                table: "BusinessEntityWorkFlowActor",
                column: "NotifierId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessEntityWorkFlowActor_Contacts_NotifierId",
                table: "BusinessEntityWorkFlowActor",
                column: "NotifierId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessEntityWorkFlowActor_AbpUsers_UserId",
                table: "BusinessEntityWorkFlowActor",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessEntityWorkFlowActor_Contacts_NotifierId",
                table: "BusinessEntityWorkFlowActor");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessEntityWorkFlowActor_AbpUsers_UserId",
                table: "BusinessEntityWorkFlowActor");

            migrationBuilder.DropIndex(
                name: "IX_BusinessEntityWorkFlowActor_NotifierId",
                table: "BusinessEntityWorkFlowActor");

            migrationBuilder.DropColumn(
                name: "NotifierId",
                table: "BusinessEntityWorkFlowActor");

            migrationBuilder.DropColumn(
                name: "NotifierUserId",
                table: "BusinessEntityWorkFlowActor");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "BusinessEntityWorkFlowActor",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessEntityWorkFlowActor_AbpUsers_UserId",
                table: "BusinessEntityWorkFlowActor",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
