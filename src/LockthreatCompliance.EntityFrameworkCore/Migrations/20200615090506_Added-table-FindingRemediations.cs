using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedtableFindingRemediations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FindingRemediations",
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
                    FindingReportId = table.Column<int>(nullable: false),
                    RemediationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FindingRemediations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FindingRemediations_FindingReports_FindingReportId",
                        column: x => x.FindingReportId,
                        principalTable: "FindingReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FindingRemediations_Remediations_RemediationId",
                        column: x => x.RemediationId,
                        principalTable: "Remediations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FindingRemediations_FindingReportId",
                table: "FindingRemediations",
                column: "FindingReportId");

            migrationBuilder.CreateIndex(
                name: "IX_FindingRemediations_RemediationId",
                table: "FindingRemediations",
                column: "RemediationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FindingRemediations");
        }
    }
}
