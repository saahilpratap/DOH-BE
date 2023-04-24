using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_QuestionsGroup_Tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestionGroups",
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
                    TenantId = table.Column<int>(nullable: true),
                    QuestionnaireTitle = table.Column<string>(nullable: true),
                    QuestionnaireType = table.Column<int>(nullable: false),
                    GroupType = table.Column<int>(nullable: false),
                    AuthoritativeDocumentId = table.Column<int>(nullable: false),
                    AuditVendorId = table.Column<long>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ControlType = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: true),
                    FacilityTypeID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionGroups_AbpUsers_AuditVendorId",
                        column: x => x.AuditVendorId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionGroups_AuthoritativeDocuments_AuthoritativeDocumentId",
                        column: x => x.AuthoritativeDocumentId,
                        principalTable: "AuthoritativeDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionGroups_AbpDynamicParameterValues_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionGroups_FacilityTypes_FacilityTypeID",
                        column: x => x.FacilityTypeID,
                        principalTable: "FacilityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionGroups_AuditVendorId",
                table: "QuestionGroups",
                column: "AuditVendorId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionGroups_AuthoritativeDocumentId",
                table: "QuestionGroups",
                column: "AuthoritativeDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionGroups_CategoryId",
                table: "QuestionGroups",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionGroups_FacilityTypeID",
                table: "QuestionGroups",
                column: "FacilityTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionGroups");
        }
    }
}
