using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class addedAudit__Contact_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditProjectTeams_AbpUsers_TeamUserId",
                table: "AuditProjectTeams");

            migrationBuilder.AlterColumn<long>(
                name: "TeamUserId",
                table: "AuditProjectTeams",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "TeamContactId",
                table: "AuditProjectTeams",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditProjectTeams_TeamContactId",
                table: "AuditProjectTeams",
                column: "TeamContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditProjectTeams_Contacts_TeamContactId",
                table: "AuditProjectTeams",
                column: "TeamContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditProjectTeams_AbpUsers_TeamUserId",
                table: "AuditProjectTeams",
                column: "TeamUserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditProjectTeams_Contacts_TeamContactId",
                table: "AuditProjectTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditProjectTeams_AbpUsers_TeamUserId",
                table: "AuditProjectTeams");

            migrationBuilder.DropIndex(
                name: "IX_AuditProjectTeams_TeamContactId",
                table: "AuditProjectTeams");

            migrationBuilder.DropColumn(
                name: "TeamContactId",
                table: "AuditProjectTeams");

            migrationBuilder.AlterColumn<long>(
                name: "TeamUserId",
                table: "AuditProjectTeams",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditProjectTeams_AbpUsers_TeamUserId",
                table: "AuditProjectTeams",
                column: "TeamUserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
