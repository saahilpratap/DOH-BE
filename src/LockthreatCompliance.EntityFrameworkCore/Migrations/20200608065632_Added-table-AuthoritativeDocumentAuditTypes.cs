using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedtableAuthoritativeDocumentAuditTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthoritativeDocumentAuditTypes",
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
                    AuthoritativeDocumentId = table.Column<int>(nullable: true),
                    AuditTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthoritativeDocumentAuditTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthoritativeDocumentAuditTypes_AbpDynamicParameterValues_AuditTypeId",
                        column: x => x.AuditTypeId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuthoritativeDocumentAuditTypes_AuthoritativeDocuments_AuthoritativeDocumentId",
                        column: x => x.AuthoritativeDocumentId,
                        principalTable: "AuthoritativeDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthoritativeDocumentAuditTypes_AuditTypeId",
                table: "AuthoritativeDocumentAuditTypes",
                column: "AuditTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthoritativeDocumentAuditTypes_AuthoritativeDocumentId",
                table: "AuthoritativeDocumentAuditTypes",
                column: "AuthoritativeDocumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthoritativeDocumentAuditTypes");
        }
    }
}
