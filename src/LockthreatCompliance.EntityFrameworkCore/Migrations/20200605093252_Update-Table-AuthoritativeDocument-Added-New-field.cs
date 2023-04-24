using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class UpdateTableAuthoritativeDocumentAddedNewfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuditTypeId",
                table: "AuthoritativeDocuments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuthorativeDocumentId",
                table: "AuthoritativeDocuments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BusinessEntityId",
                table: "AuthoritativeDocuments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "AuthoritativeDocuments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthoritativeDocuments_AuditTypeId",
                table: "AuthoritativeDocuments",
                column: "AuditTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthoritativeDocuments_BusinessEntityId",
                table: "AuthoritativeDocuments",
                column: "BusinessEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthoritativeDocuments_CategoryId",
                table: "AuthoritativeDocuments",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthoritativeDocuments_AbpDynamicParameterValues_AuditTypeId",
                table: "AuthoritativeDocuments",
                column: "AuditTypeId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthoritativeDocuments_BusinessEntities_BusinessEntityId",
                table: "AuthoritativeDocuments",
                column: "BusinessEntityId",
                principalTable: "BusinessEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthoritativeDocuments_AbpDynamicParameterValues_CategoryId",
                table: "AuthoritativeDocuments",
                column: "CategoryId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthoritativeDocuments_AbpDynamicParameterValues_AuditTypeId",
                table: "AuthoritativeDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthoritativeDocuments_BusinessEntities_BusinessEntityId",
                table: "AuthoritativeDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthoritativeDocuments_AbpDynamicParameterValues_CategoryId",
                table: "AuthoritativeDocuments");

            migrationBuilder.DropIndex(
                name: "IX_AuthoritativeDocuments_AuditTypeId",
                table: "AuthoritativeDocuments");

            migrationBuilder.DropIndex(
                name: "IX_AuthoritativeDocuments_BusinessEntityId",
                table: "AuthoritativeDocuments");

            migrationBuilder.DropIndex(
                name: "IX_AuthoritativeDocuments_CategoryId",
                table: "AuthoritativeDocuments");

            migrationBuilder.DropColumn(
                name: "AuditTypeId",
                table: "AuthoritativeDocuments");

            migrationBuilder.DropColumn(
                name: "AuthorativeDocumentId",
                table: "AuthoritativeDocuments");

            migrationBuilder.DropColumn(
                name: "BusinessEntityId",
                table: "AuthoritativeDocuments");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "AuthoritativeDocuments");
        }
    }
}
