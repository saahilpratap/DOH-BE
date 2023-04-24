using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_PatientAuthenticationPlatformContactInformation_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientAuthenticationPlatforms_AbpDynamicParameterValues_EnvironmentId",
                table: "PatientAuthenticationPlatforms");

            migrationBuilder.DropIndex(
                name: "IX_PatientAuthenticationPlatforms_EnvironmentId",
                table: "PatientAuthenticationPlatforms");

            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "PatientAuthenticationPlatforms");

            migrationBuilder.DropColumn(
                name: "EnvironmentId",
                table: "PatientAuthenticationPlatforms");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "PatientAuthenticationPlatforms");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "PatientAuthenticationPlatforms");

            migrationBuilder.DropColumn(
                name: "MobilePhoneNumber",
                table: "PatientAuthenticationPlatforms");

            migrationBuilder.DropColumn(
                name: "ProviderName",
                table: "PatientAuthenticationPlatforms");

            migrationBuilder.DropColumn(
                name: "TargetDate",
                table: "PatientAuthenticationPlatforms");

            migrationBuilder.AddColumn<bool>(
                name: "Connecting",
                table: "PatientAuthenticationPlatforms",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "GroupName",
                table: "PatientAuthenticationPlatforms",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PatientAuthenticationPlatformContactInformation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    MobilePhoneNumber = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    PatientAuthenticationPlatformId = table.Column<int>(nullable: false),
                    PatientAuthenticationPlatformId1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAuthenticationPlatformContactInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientAuthenticationPlatformContactInformation_PatientAuthenticationPlatforms_PatientAuthenticationPlatformId1",
                        column: x => x.PatientAuthenticationPlatformId1,
                        principalTable: "PatientAuthenticationPlatforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientAuthenticationPlatformContactInformation_PatientAuthenticationPlatformId1",
                table: "PatientAuthenticationPlatformContactInformation",
                column: "PatientAuthenticationPlatformId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientAuthenticationPlatformContactInformation");

            migrationBuilder.DropColumn(
                name: "Connecting",
                table: "PatientAuthenticationPlatforms");

            migrationBuilder.DropColumn(
                name: "GroupName",
                table: "PatientAuthenticationPlatforms");

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "PatientAuthenticationPlatforms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EnvironmentId",
                table: "PatientAuthenticationPlatforms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "PatientAuthenticationPlatforms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "PatientAuthenticationPlatforms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MobilePhoneNumber",
                table: "PatientAuthenticationPlatforms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProviderName",
                table: "PatientAuthenticationPlatforms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TargetDate",
                table: "PatientAuthenticationPlatforms",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientAuthenticationPlatforms_EnvironmentId",
                table: "PatientAuthenticationPlatforms",
                column: "EnvironmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAuthenticationPlatforms_AbpDynamicParameterValues_EnvironmentId",
                table: "PatientAuthenticationPlatforms",
                column: "EnvironmentId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
