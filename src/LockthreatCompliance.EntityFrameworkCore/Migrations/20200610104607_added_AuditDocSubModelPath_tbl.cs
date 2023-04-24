using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_AuditDocSubModelPath_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditDocSubModelsPath",
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
                    FileName = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    AuditMeetingId = table.Column<long>(nullable: true),
                    AuditChecklistId = table.Column<long>(nullable: true),
                    AuditProcedureId = table.Column<long>(nullable: true),
                    AuditTemplateId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditDocSubModelsPath", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditDocSubModelsPath_AuditChecklists_AuditChecklistId",
                        column: x => x.AuditChecklistId,
                        principalTable: "AuditChecklists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditDocSubModelsPath_AuditMeetings_AuditMeetingId",
                        column: x => x.AuditMeetingId,
                        principalTable: "AuditMeetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditDocSubModelsPath_AuditProcedures_AuditProcedureId",
                        column: x => x.AuditProcedureId,
                        principalTable: "AuditProcedures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditDocSubModelsPath_AuditTemplates_AuditTemplateId",
                        column: x => x.AuditTemplateId,
                        principalTable: "AuditTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditDocSubModelsPath_AuditChecklistId",
                table: "AuditDocSubModelsPath",
                column: "AuditChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditDocSubModelsPath_AuditMeetingId",
                table: "AuditDocSubModelsPath",
                column: "AuditMeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditDocSubModelsPath_AuditProcedureId",
                table: "AuditDocSubModelsPath",
                column: "AuditProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditDocSubModelsPath_AuditTemplateId",
                table: "AuditDocSubModelsPath",
                column: "AuditTemplateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditDocSubModelsPath");
        }
    }
}
