using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updatetableAuditMeetingaddednewcolumsMeetingStage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MeetingStageId",
                table: "AuditMeetings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditMeetings_MeetingStageId",
                table: "AuditMeetings",
                column: "MeetingStageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditMeetings_AbpDynamicParameterValues_MeetingStageId",
                table: "AuditMeetings",
                column: "MeetingStageId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditMeetings_AbpDynamicParameterValues_MeetingStageId",
                table: "AuditMeetings");

            migrationBuilder.DropIndex(
                name: "IX_AuditMeetings_MeetingStageId",
                table: "AuditMeetings");

            migrationBuilder.DropColumn(
                name: "MeetingStageId",
                table: "AuditMeetings");
        }
    }
}
