using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_FacilityTypeSizeSetting_Tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FacilityTypeSizeSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    FacilityTypeId = table.Column<int>(nullable: false),
                    AppSettingId = table.Column<int>(nullable: false),
                    EntityApplicationSettingId = table.Column<int>(nullable: true),
                    MinSize = table.Column<int>(nullable: false),
                    MaxSize = table.Column<int>(nullable: false),
                    IsSelected = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityTypeSizeSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacilityTypeSizeSettings_EntityApplicationSettings_EntityApplicationSettingId",
                        column: x => x.EntityApplicationSettingId,
                        principalTable: "EntityApplicationSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FacilityTypeSizeSettings_FacilityTypes_FacilityTypeId",
                        column: x => x.FacilityTypeId,
                        principalTable: "FacilityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacilityTypeSizeSettings_EntityApplicationSettingId",
                table: "FacilityTypeSizeSettings",
                column: "EntityApplicationSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityTypeSizeSettings_FacilityTypeId",
                table: "FacilityTypeSizeSettings",
                column: "FacilityTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FacilityTypeSizeSettings");
        }
    }
}
