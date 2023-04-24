using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedtableAuditDecUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditDecUsers",
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
                    AuditDecFormId = table.Column<int>(nullable: true),
                    MemberNameId = table.Column<long>(nullable: true),
                    Approved = table.Column<bool>(nullable: false),
                    Signature = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditDecUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditDecUsers_AuditDecForms_AuditDecFormId",
                        column: x => x.AuditDecFormId,
                        principalTable: "AuditDecForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditDecUsers_AbpUsers_MemberNameId",
                        column: x => x.MemberNameId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditDecUsers_AuditDecFormId",
                table: "AuditDecUsers",
                column: "AuditDecFormId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditDecUsers_MemberNameId",
                table: "AuditDecUsers",
                column: "MemberNameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditDecUsers");
        }
    }
}
