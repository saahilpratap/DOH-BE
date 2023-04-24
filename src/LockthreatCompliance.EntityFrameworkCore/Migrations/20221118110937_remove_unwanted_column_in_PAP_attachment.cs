using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class remove_unwanted_column_in_PAP_attachment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientAuthenticationPlatformAttachments_PatientAuthenticationPlatforms_PatientAuthenticationPlatformId1",
                table: "PatientAuthenticationPlatformAttachments");

            migrationBuilder.DropIndex(
                name: "IX_PatientAuthenticationPlatformAttachments_PatientAuthenticationPlatformId1",
                table: "PatientAuthenticationPlatformAttachments");

            migrationBuilder.DropColumn(
                name: "PatientAuthenticationPlatformId",
                table: "PatientAuthenticationPlatformAttachments");

            migrationBuilder.DropColumn(
                name: "PatientAuthenticationPlatformId1",
                table: "PatientAuthenticationPlatformAttachments");

            migrationBuilder.AddColumn<long>(
                name: "PAPId",
                table: "PatientAuthenticationPlatformAttachments",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_PatientAuthenticationPlatformAttachments_PAPId",
                table: "PatientAuthenticationPlatformAttachments",
                column: "PAPId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAuthenticationPlatformAttachments_PatientAuthenticationPlatforms_PAPId",
                table: "PatientAuthenticationPlatformAttachments",
                column: "PAPId",
                principalTable: "PatientAuthenticationPlatforms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientAuthenticationPlatformAttachments_PatientAuthenticationPlatforms_PAPId",
                table: "PatientAuthenticationPlatformAttachments");

            migrationBuilder.DropIndex(
                name: "IX_PatientAuthenticationPlatformAttachments_PAPId",
                table: "PatientAuthenticationPlatformAttachments");

            migrationBuilder.DropColumn(
                name: "PAPId",
                table: "PatientAuthenticationPlatformAttachments");

            migrationBuilder.AddColumn<int>(
                name: "PatientAuthenticationPlatformId",
                table: "PatientAuthenticationPlatformAttachments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PatientAuthenticationPlatformId1",
                table: "PatientAuthenticationPlatformAttachments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientAuthenticationPlatformAttachments_PatientAuthenticationPlatformId1",
                table: "PatientAuthenticationPlatformAttachments",
                column: "PatientAuthenticationPlatformId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAuthenticationPlatformAttachments_PatientAuthenticationPlatforms_PatientAuthenticationPlatformId1",
                table: "PatientAuthenticationPlatformAttachments",
                column: "PatientAuthenticationPlatformId1",
                principalTable: "PatientAuthenticationPlatforms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
