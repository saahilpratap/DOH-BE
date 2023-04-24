using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updated_QuestionGrp_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionGroups_AbpUsers_AuditVendorId",
                table: "QuestionGroups");

            migrationBuilder.AlterColumn<int>(
                name: "AuditVendorId",
                table: "QuestionGroups",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionGroups_BusinessEntities_AuditVendorId",
                table: "QuestionGroups",
                column: "AuditVendorId",
                principalTable: "BusinessEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionGroups_BusinessEntities_AuditVendorId",
                table: "QuestionGroups");

            migrationBuilder.AlterColumn<long>(
                name: "AuditVendorId",
                table: "QuestionGroups",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionGroups_AbpUsers_AuditVendorId",
                table: "QuestionGroups",
                column: "AuditVendorId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
