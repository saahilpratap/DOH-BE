using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_PatientAuthenticationPlatforms_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientAuthenticationPlatforms",
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
                    EnvironmentId = table.Column<int>(nullable: true),
                    TargetDate = table.Column<DateTime>(nullable: true),
                    ProviderName = table.Column<string>(nullable: true),
                    FacilityLicenseNumber = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    MobilePhoneNumber = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    AdditionalInformation = table.Column<string>(nullable: true),
                    StatusId = table.Column<int>(nullable: true),
                    Comment1 = table.Column<string>(nullable: true),
                    Comment2 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAuthenticationPlatforms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientAuthenticationPlatforms_AbpDynamicParameterValues_EnvironmentId",
                        column: x => x.EnvironmentId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientAuthenticationPlatforms_AbpDynamicParameterValues_StatusId",
                        column: x => x.StatusId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientAuthenticationPlatforms_EnvironmentId",
                table: "PatientAuthenticationPlatforms",
                column: "EnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAuthenticationPlatforms_StatusId",
                table: "PatientAuthenticationPlatforms",
                column: "StatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientAuthenticationPlatforms");
        }
    }
}
