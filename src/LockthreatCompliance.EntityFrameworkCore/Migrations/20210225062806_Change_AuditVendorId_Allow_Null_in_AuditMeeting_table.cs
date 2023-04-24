using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class Change_AuditVendorId_Allow_Null_in_AuditMeeting_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditMeetings_BusinessEntities_AuditVendorId",
                table: "AuditMeetings");

            migrationBuilder.AlterColumn<int>(
                name: "AuditVendorId",
                table: "AuditMeetings",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditMeetings_BusinessEntities_AuditVendorId",
                table: "AuditMeetings",
                column: "AuditVendorId",
                principalTable: "BusinessEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditMeetings_BusinessEntities_AuditVendorId",
                table: "AuditMeetings");

            migrationBuilder.AlterColumn<int>(
                name: "AuditVendorId",
                table: "AuditMeetings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditMeetings_BusinessEntities_AuditVendorId",
                table: "AuditMeetings",
                column: "AuditVendorId",
                principalTable: "BusinessEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
