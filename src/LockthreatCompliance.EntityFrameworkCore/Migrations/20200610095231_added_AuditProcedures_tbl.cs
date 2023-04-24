using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_AuditProcedures_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AuditProcedureId",
                table: "DocumentPaths",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuditProcedures",
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
                    ProcedureTitle = table.Column<string>(nullable: true),
                    AppEntityType = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 9999, nullable: true),
                    Result = table.Column<string>(nullable: true),
                    Recommendation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditProcedures", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPaths_AuditProcedureId",
                table: "DocumentPaths",
                column: "AuditProcedureId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentPaths_AuditProcedures_AuditProcedureId",
                table: "DocumentPaths",
                column: "AuditProcedureId",
                principalTable: "AuditProcedures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentPaths_AuditProcedures_AuditProcedureId",
                table: "DocumentPaths");

            migrationBuilder.DropTable(
                name: "AuditProcedures");

            migrationBuilder.DropIndex(
                name: "IX_DocumentPaths_AuditProcedureId",
                table: "DocumentPaths");

            migrationBuilder.DropColumn(
                name: "AuditProcedureId",
                table: "DocumentPaths");
        }
    }
}
