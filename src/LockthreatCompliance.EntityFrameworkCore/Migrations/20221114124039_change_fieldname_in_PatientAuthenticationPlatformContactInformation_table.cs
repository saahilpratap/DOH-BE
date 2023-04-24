using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class change_fieldname_in_PatientAuthenticationPlatformContactInformation_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientAuthenticationPlatformContactInformation_PatientAuthenticationPlatforms_PatientAuthenticationPlatformId1",
                table: "PatientAuthenticationPlatformContactInformation");

            migrationBuilder.DropIndex(
                name: "IX_PatientAuthenticationPlatformContactInformation_PatientAuthenticationPlatformId1",
                table: "PatientAuthenticationPlatformContactInformation");

            migrationBuilder.DropColumn(
                name: "PatientAuthenticationPlatformId",
                table: "PatientAuthenticationPlatformContactInformation");

            migrationBuilder.DropColumn(
                name: "PatientAuthenticationPlatformId1",
                table: "PatientAuthenticationPlatformContactInformation");

            migrationBuilder.AddColumn<long>(
                name: "PAPId",
                table: "PatientAuthenticationPlatformContactInformation",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_PatientAuthenticationPlatformContactInformation_PAPId",
                table: "PatientAuthenticationPlatformContactInformation",
                column: "PAPId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAuthenticationPlatformContactInformation_PatientAuthenticationPlatforms_PAPId",
                table: "PatientAuthenticationPlatformContactInformation",
                column: "PAPId",
                principalTable: "PatientAuthenticationPlatforms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientAuthenticationPlatformContactInformation_PatientAuthenticationPlatforms_PAPId",
                table: "PatientAuthenticationPlatformContactInformation");

            migrationBuilder.DropIndex(
                name: "IX_PatientAuthenticationPlatformContactInformation_PAPId",
                table: "PatientAuthenticationPlatformContactInformation");

            migrationBuilder.DropColumn(
                name: "PAPId",
                table: "PatientAuthenticationPlatformContactInformation");

            migrationBuilder.AddColumn<int>(
                name: "PatientAuthenticationPlatformId",
                table: "PatientAuthenticationPlatformContactInformation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "PatientAuthenticationPlatformId1",
                table: "PatientAuthenticationPlatformContactInformation",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientAuthenticationPlatformContactInformation_PatientAuthenticationPlatformId1",
                table: "PatientAuthenticationPlatformContactInformation",
                column: "PatientAuthenticationPlatformId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAuthenticationPlatformContactInformation_PatientAuthenticationPlatforms_PatientAuthenticationPlatformId1",
                table: "PatientAuthenticationPlatformContactInformation",
                column: "PatientAuthenticationPlatformId1",
                principalTable: "PatientAuthenticationPlatforms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
