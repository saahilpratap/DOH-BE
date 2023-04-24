﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_PatientAuthenticationPlatformSelectedEntities_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientAuthenticationPlatformSelectedEntities",
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
                    BusinessEntityId = table.Column<int>(nullable: false),
                    PAPId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAuthenticationPlatformSelectedEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientAuthenticationPlatformSelectedEntities_BusinessEntities_BusinessEntityId",
                        column: x => x.BusinessEntityId,
                        principalTable: "BusinessEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientAuthenticationPlatformSelectedEntities_PatientAuthenticationPlatforms_PAPId",
                        column: x => x.PAPId,
                        principalTable: "PatientAuthenticationPlatforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientAuthenticationPlatformSelectedEntities_BusinessEntityId",
                table: "PatientAuthenticationPlatformSelectedEntities",
                column: "BusinessEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAuthenticationPlatformSelectedEntities_PAPId",
                table: "PatientAuthenticationPlatformSelectedEntities",
                column: "PAPId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientAuthenticationPlatformSelectedEntities");
        }
    }
}
