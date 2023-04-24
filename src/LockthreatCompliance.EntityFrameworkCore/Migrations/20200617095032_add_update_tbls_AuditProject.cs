using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class add_update_tbls_AuditProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TemplateChecklistId",
                table: "AuditDocSubModelsPath",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuditProjectAuthoritativeDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthoritativeDocumentId = table.Column<int>(nullable: false),
                    AuditProjectId = table.Column<int>(nullable: false),
                    AuditProjectId1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditProjectAuthoritativeDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditProjectAuthoritativeDocuments_AuditProjects_AuditProjectId1",
                        column: x => x.AuditProjectId1,
                        principalTable: "AuditProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditProjectAuthoritativeDocuments_AuthoritativeDocuments_AuthoritativeDocumentId",
                        column: x => x.AuthoritativeDocumentId,
                        principalTable: "AuthoritativeDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemplateChecklists",
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
                    Title = table.Column<string>(nullable: true),
                    AppEntityType = table.Column<int>(nullable: false),
                    InSystem = table.Column<string>(maxLength: 9999, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    VendorId = table.Column<int>(nullable: true),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    CategoryId = table.Column<int>(nullable: true),
                    FacilityTypeId = table.Column<int>(nullable: true),
                    TemplateTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateChecklists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateChecklists_AbpDynamicParameterValues_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TemplateChecklists_FacilityTypes_FacilityTypeId",
                        column: x => x.FacilityTypeId,
                        principalTable: "FacilityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TemplateChecklists_AbpDynamicParameterValues_TemplateTypeId",
                        column: x => x.TemplateTypeId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TemplateChecklists_BusinessEntities_VendorId",
                        column: x => x.VendorId,
                        principalTable: "BusinessEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TemplateChecklistAuthoritativeDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthoritativeDocumentId = table.Column<int>(nullable: false),
                    TemplateChecklistId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateChecklistAuthoritativeDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateChecklistAuthoritativeDocuments_AuthoritativeDocuments_AuthoritativeDocumentId",
                        column: x => x.AuthoritativeDocumentId,
                        principalTable: "AuthoritativeDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemplateChecklistAuthoritativeDocuments_TemplateChecklists_TemplateChecklistId",
                        column: x => x.TemplateChecklistId,
                        principalTable: "TemplateChecklists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessEntities_CategoryId",
                table: "BusinessEntities",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditDocSubModelsPath_TemplateChecklistId",
                table: "AuditDocSubModelsPath",
                column: "TemplateChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditProjectAuthoritativeDocuments_AuditProjectId1",
                table: "AuditProjectAuthoritativeDocuments",
                column: "AuditProjectId1");

            migrationBuilder.CreateIndex(
                name: "IX_AuditProjectAuthoritativeDocuments_AuthoritativeDocumentId",
                table: "AuditProjectAuthoritativeDocuments",
                column: "AuthoritativeDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateChecklistAuthoritativeDocuments_AuthoritativeDocumentId",
                table: "TemplateChecklistAuthoritativeDocuments",
                column: "AuthoritativeDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateChecklistAuthoritativeDocuments_TemplateChecklistId",
                table: "TemplateChecklistAuthoritativeDocuments",
                column: "TemplateChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateChecklists_CategoryId",
                table: "TemplateChecklists",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateChecklists_FacilityTypeId",
                table: "TemplateChecklists",
                column: "FacilityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateChecklists_TemplateTypeId",
                table: "TemplateChecklists",
                column: "TemplateTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateChecklists_VendorId",
                table: "TemplateChecklists",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditDocSubModelsPath_TemplateChecklists_TemplateChecklistId",
                table: "AuditDocSubModelsPath",
                column: "TemplateChecklistId",
                principalTable: "TemplateChecklists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessEntities_AbpDynamicParameterValues_CategoryId",
                table: "BusinessEntities",
                column: "CategoryId",
                principalTable: "AbpDynamicParameterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditDocSubModelsPath_TemplateChecklists_TemplateChecklistId",
                table: "AuditDocSubModelsPath");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessEntities_AbpDynamicParameterValues_CategoryId",
                table: "BusinessEntities");

            migrationBuilder.DropTable(
                name: "AuditProjectAuthoritativeDocuments");

            migrationBuilder.DropTable(
                name: "TemplateChecklistAuthoritativeDocuments");

            migrationBuilder.DropTable(
                name: "TemplateChecklists");

            migrationBuilder.DropIndex(
                name: "IX_BusinessEntities_CategoryId",
                table: "BusinessEntities");

            migrationBuilder.DropIndex(
                name: "IX_AuditDocSubModelsPath_TemplateChecklistId",
                table: "AuditDocSubModelsPath");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "TemplateChecklistId",
                table: "AuditDocSubModelsPath");
        }
    }
}
