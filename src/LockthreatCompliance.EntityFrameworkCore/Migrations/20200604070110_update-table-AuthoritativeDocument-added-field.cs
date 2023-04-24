using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updatetableAuthoritativeDocumentaddedfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthoratativeDocumentLogo",
                table: "AuthoritativeDocuments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AuthoritativeDocuments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentTypeId",
                table: "AuthoritativeDocuments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "AuthoritativeDocuments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthoritativeDocuments_DocumentTypeId",
                table: "AuthoritativeDocuments",
                column: "DocumentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthoritativeDocuments_AbpDynamicParameterValues_DocumentTypeId",
                table: "AuthoritativeDocuments",
                column: "DocumentTypeId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthoritativeDocuments_AbpDynamicParameterValues_DocumentTypeId",
                table: "AuthoritativeDocuments");

            migrationBuilder.DropIndex(
                name: "IX_AuthoritativeDocuments_DocumentTypeId",
                table: "AuthoritativeDocuments");

            migrationBuilder.DropColumn(
                name: "AuthoratativeDocumentLogo",
                table: "AuthoritativeDocuments");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AuthoritativeDocuments");

            migrationBuilder.DropColumn(
                name: "DocumentTypeId",
                table: "AuthoritativeDocuments");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "AuthoritativeDocuments");
        }
    }
}
