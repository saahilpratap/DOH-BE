using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class add_two_column_in_PreRegisterBusinessEntity_in_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalAssessmentAuthoritativeDocument_AuthoritativeDocuments_AuthoritativeDocumentId",
                table: "ExternalAssessmentAuthoritativeDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_ExternalAssessmentAuthoritativeDocument_ExternalAssessments_ExternalAssessmentId",
                table: "ExternalAssessmentAuthoritativeDocument");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExternalAssessmentAuthoritativeDocument",
                table: "ExternalAssessmentAuthoritativeDocument");

            migrationBuilder.RenameTable(
                name: "ExternalAssessmentAuthoritativeDocument",
                newName: "ExternalAssessmentAuthoritativeDocuments");

            migrationBuilder.RenameIndex(
                name: "IX_ExternalAssessmentAuthoritativeDocument_ExternalAssessmentId",
                table: "ExternalAssessmentAuthoritativeDocuments",
                newName: "IX_ExternalAssessmentAuthoritativeDocuments_ExternalAssessmentId");

            migrationBuilder.RenameIndex(
                name: "IX_ExternalAssessmentAuthoritativeDocument_AuthoritativeDocumentId",
                table: "ExternalAssessmentAuthoritativeDocuments",
                newName: "IX_ExternalAssessmentAuthoritativeDocuments_AuthoritativeDocumentId");

            migrationBuilder.AddColumn<string>(
                name: "AdminName",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminSurname",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExternalAssessmentAuthoritativeDocuments",
                table: "ExternalAssessmentAuthoritativeDocuments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalAssessmentAuthoritativeDocuments_AuthoritativeDocuments_AuthoritativeDocumentId",
                table: "ExternalAssessmentAuthoritativeDocuments",
                column: "AuthoritativeDocumentId",
                principalTable: "AuthoritativeDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalAssessmentAuthoritativeDocuments_ExternalAssessments_ExternalAssessmentId",
                table: "ExternalAssessmentAuthoritativeDocuments",
                column: "ExternalAssessmentId",
                principalTable: "ExternalAssessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalAssessmentAuthoritativeDocuments_AuthoritativeDocuments_AuthoritativeDocumentId",
                table: "ExternalAssessmentAuthoritativeDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_ExternalAssessmentAuthoritativeDocuments_ExternalAssessments_ExternalAssessmentId",
                table: "ExternalAssessmentAuthoritativeDocuments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExternalAssessmentAuthoritativeDocuments",
                table: "ExternalAssessmentAuthoritativeDocuments");

            migrationBuilder.DropColumn(
                name: "AdminName",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "AdminSurname",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.RenameTable(
                name: "ExternalAssessmentAuthoritativeDocuments",
                newName: "ExternalAssessmentAuthoritativeDocument");

            migrationBuilder.RenameIndex(
                name: "IX_ExternalAssessmentAuthoritativeDocuments_ExternalAssessmentId",
                table: "ExternalAssessmentAuthoritativeDocument",
                newName: "IX_ExternalAssessmentAuthoritativeDocument_ExternalAssessmentId");

            migrationBuilder.RenameIndex(
                name: "IX_ExternalAssessmentAuthoritativeDocuments_AuthoritativeDocumentId",
                table: "ExternalAssessmentAuthoritativeDocument",
                newName: "IX_ExternalAssessmentAuthoritativeDocument_AuthoritativeDocumentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExternalAssessmentAuthoritativeDocument",
                table: "ExternalAssessmentAuthoritativeDocument",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalAssessmentAuthoritativeDocument_AuthoritativeDocuments_AuthoritativeDocumentId",
                table: "ExternalAssessmentAuthoritativeDocument",
                column: "AuthoritativeDocumentId",
                principalTable: "AuthoritativeDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalAssessmentAuthoritativeDocument_ExternalAssessments_ExternalAssessmentId",
                table: "ExternalAssessmentAuthoritativeDocument",
                column: "ExternalAssessmentId",
                principalTable: "ExternalAssessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
