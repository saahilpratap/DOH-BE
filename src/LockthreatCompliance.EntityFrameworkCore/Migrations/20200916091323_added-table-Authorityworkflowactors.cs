using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class addedtableAuthorityworkflowactors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authorityworkflowactors",
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
                    TenantId = table.Column<int>(nullable: true),
                    WorkFlowNameId = table.Column<int>(nullable: true),
                    AuthorityDepartmentId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    NotifierUserId = table.Column<int>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    IsPrimaryUser = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    AuthoritativeDocumentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorityworkflowactors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Authorityworkflowactors_AuthoritativeDocuments_AuthoritativeDocumentId",
                        column: x => x.AuthoritativeDocumentId,
                        principalTable: "AuthoritativeDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Authorityworkflowactors_AuthorityDepartments_AuthorityDepartmentId",
                        column: x => x.AuthorityDepartmentId,
                        principalTable: "AuthorityDepartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Authorityworkflowactors_Contacts_NotifierUserId",
                        column: x => x.NotifierUserId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Authorityworkflowactors_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Authorityworkflowactors_AbpDynamicParameterValues_WorkFlowNameId",
                        column: x => x.WorkFlowNameId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authorityworkflowactors_AuthoritativeDocumentId",
                table: "Authorityworkflowactors",
                column: "AuthoritativeDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorityworkflowactors_AuthorityDepartmentId",
                table: "Authorityworkflowactors",
                column: "AuthorityDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorityworkflowactors_NotifierUserId",
                table: "Authorityworkflowactors",
                column: "NotifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorityworkflowactors_UserId",
                table: "Authorityworkflowactors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorityworkflowactors_WorkFlowNameId",
                table: "Authorityworkflowactors",
                column: "WorkFlowNameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Authorityworkflowactors");
        }
    }
}
