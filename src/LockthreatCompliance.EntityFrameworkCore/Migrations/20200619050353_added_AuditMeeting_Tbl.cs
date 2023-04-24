using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_AuditMeeting_Tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditMeetings_AuditProjects_AuditProjectId",
                table: "AuditMeetings");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentPaths_AuditMeetings_AuditMeetingId",
                table: "DocumentPaths");

            migrationBuilder.DropIndex(
                name: "IX_DocumentPaths_AuditMeetingId",
                table: "DocumentPaths");

            migrationBuilder.DropColumn(
                name: "AuditMeetingId",
                table: "DocumentPaths");

            migrationBuilder.AlterColumn<long>(
                name: "AuditProjectId",
                table: "AuditMeetings",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuditOrgId",
                table: "AuditMeetings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuditVendorId",
                table: "AuditMeetings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AuditMeetings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MeetingTypeId",
                table: "AuditMeetings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MeetingVenue",
                table: "AuditMeetings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditMeetings_AuditOrgId",
                table: "AuditMeetings",
                column: "AuditOrgId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditMeetings_AuditVendorId",
                table: "AuditMeetings",
                column: "AuditVendorId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditMeetings_MeetingTypeId",
                table: "AuditMeetings",
                column: "MeetingTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditMeetings_BusinessEntities_AuditOrgId",
                table: "AuditMeetings",
                column: "AuditOrgId",
                principalTable: "BusinessEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditMeetings_AuditProjects_AuditProjectId",
                table: "AuditMeetings",
                column: "AuditProjectId",
                principalTable: "AuditProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditMeetings_BusinessEntities_AuditVendorId",
                table: "AuditMeetings",
                column: "AuditVendorId",
                principalTable: "BusinessEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditMeetings_AbpDynamicParameterValues_MeetingTypeId",
                table: "AuditMeetings",
                column: "MeetingTypeId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditMeetings_BusinessEntities_AuditOrgId",
                table: "AuditMeetings");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditMeetings_AuditProjects_AuditProjectId",
                table: "AuditMeetings");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditMeetings_BusinessEntities_AuditVendorId",
                table: "AuditMeetings");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditMeetings_AbpDynamicParameterValues_MeetingTypeId",
                table: "AuditMeetings");

            migrationBuilder.DropIndex(
                name: "IX_AuditMeetings_AuditOrgId",
                table: "AuditMeetings");

            migrationBuilder.DropIndex(
                name: "IX_AuditMeetings_AuditVendorId",
                table: "AuditMeetings");

            migrationBuilder.DropIndex(
                name: "IX_AuditMeetings_MeetingTypeId",
                table: "AuditMeetings");

            migrationBuilder.DropColumn(
                name: "AuditOrgId",
                table: "AuditMeetings");

            migrationBuilder.DropColumn(
                name: "AuditVendorId",
                table: "AuditMeetings");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AuditMeetings");

            migrationBuilder.DropColumn(
                name: "MeetingTypeId",
                table: "AuditMeetings");

            migrationBuilder.DropColumn(
                name: "MeetingVenue",
                table: "AuditMeetings");

            migrationBuilder.AddColumn<long>(
                name: "AuditMeetingId",
                table: "DocumentPaths",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "AuditProjectId",
                table: "AuditMeetings",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPaths_AuditMeetingId",
                table: "DocumentPaths",
                column: "AuditMeetingId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditMeetings_AuditProjects_AuditProjectId",
                table: "AuditMeetings",
                column: "AuditProjectId",
                principalTable: "AuditProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentPaths_AuditMeetings_AuditMeetingId",
                table: "DocumentPaths",
                column: "AuditMeetingId",
                principalTable: "AuditMeetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
