using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class REmove_colmns_AuthDoc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthoritativeDocuments_AbpDynamicParameterValues_AuditTypeId",
                table: "AuthoritativeDocuments");

            migrationBuilder.DropIndex(
                name: "IX_AuthoritativeDocuments_AuditTypeId",
                table: "AuthoritativeDocuments");

            migrationBuilder.DropColumn(
                name: "AuditTypeId",
                table: "AuthoritativeDocuments");

            migrationBuilder.DropColumn(
                name: "AuthorativeDocumentId",
                table: "AuthoritativeDocuments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuditTypeId",
                table: "AuthoritativeDocuments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuthorativeDocumentId",
                table: "AuthoritativeDocuments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthoritativeDocuments_AuditTypeId",
                table: "AuthoritativeDocuments",
                column: "AuditTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthoritativeDocuments_AbpDynamicParameterValues_AuditTypeId",
                table: "AuthoritativeDocuments",
                column: "AuditTypeId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
