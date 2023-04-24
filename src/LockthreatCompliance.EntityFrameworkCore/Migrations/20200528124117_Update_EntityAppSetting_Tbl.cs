using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class Update_EntityAppSetting_Tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "EntityApplicationSettings",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "EntityApplicationSettings",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "EntityApplicationSettings",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "EntityApplicationSettings",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EntityApplicationSettings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "EntityApplicationSettings",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "EntityApplicationSettings",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EntityApplicationSettingsCustomWorkFlowTypes",
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
                    AppSettingId = table.Column<int>(nullable: false),
                    EntityApplicationSettingId = table.Column<int>(nullable: true),
                    SelectedStatusId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityApplicationSettingsCustomWorkFlowTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityApplicationSettingsCustomWorkFlowTypes_EntityApplicationSettings_EntityApplicationSettingId",
                        column: x => x.EntityApplicationSettingId,
                        principalTable: "EntityApplicationSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntityApplicationSettingsCustomWorkFlowTypes_EntityApplicationSettingId",
                table: "EntityApplicationSettingsCustomWorkFlowTypes",
                column: "EntityApplicationSettingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityApplicationSettingsCustomWorkFlowTypes");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "EntityApplicationSettings");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "EntityApplicationSettings");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "EntityApplicationSettings");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "EntityApplicationSettings");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EntityApplicationSettings");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "EntityApplicationSettings");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "EntityApplicationSettings");
        }
    }
}
