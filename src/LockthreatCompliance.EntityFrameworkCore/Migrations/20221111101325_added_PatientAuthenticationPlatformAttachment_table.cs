using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_PatientAuthenticationPlatformAttachment_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientAuthenticationPlatformAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    PAPAttachmentType = table.Column<int>(nullable: false),
                    PatientAuthenticationPlatformId = table.Column<int>(nullable: true),
                    PatientAuthenticationPlatformId1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAuthenticationPlatformAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientAuthenticationPlatformAttachments_PatientAuthenticationPlatforms_PatientAuthenticationPlatformId1",
                        column: x => x.PatientAuthenticationPlatformId1,
                        principalTable: "PatientAuthenticationPlatforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientAuthenticationPlatformAttachments_PatientAuthenticationPlatformId1",
                table: "PatientAuthenticationPlatformAttachments",
                column: "PatientAuthenticationPlatformId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientAuthenticationPlatformAttachments");
        }
    }
}
