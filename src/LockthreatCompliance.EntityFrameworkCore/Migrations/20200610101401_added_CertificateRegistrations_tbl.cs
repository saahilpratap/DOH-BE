using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_CertificateRegistrations_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CertificateRegistrations",
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
                    BusinessEntityId = table.Column<int>(nullable: false),
                    AuditProjectId = table.Column<long>(nullable: true),
                    IssuedDate = table.Column<DateTime>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    AuditUserId = table.Column<long>(nullable: false),
                    AuditSign = table.Column<string>(maxLength: 9999, nullable: true),
                    HEUserId = table.Column<long>(nullable: false),
                    HESign = table.Column<string>(maxLength: 9999, nullable: true),
                    AuthUserId = table.Column<long>(nullable: false),
                    AuthSign = table.Column<string>(maxLength: 9999, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CertificateRegistrations_AuditProjects_AuditProjectId",
                        column: x => x.AuditProjectId,
                        principalTable: "AuditProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CertificateRegistrations_AbpUsers_AuditUserId",
                        column: x => x.AuditUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CertificateRegistrations_AbpUsers_AuthUserId",
                        column: x => x.AuthUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CertificateRegistrations_BusinessEntities_BusinessEntityId",
                        column: x => x.BusinessEntityId,
                        principalTable: "BusinessEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CertificateRegistrations_AbpUsers_HEUserId",
                        column: x => x.HEUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CertificateRegistrations_AuditProjectId",
                table: "CertificateRegistrations",
                column: "AuditProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateRegistrations_AuditUserId",
                table: "CertificateRegistrations",
                column: "AuditUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateRegistrations_AuthUserId",
                table: "CertificateRegistrations",
                column: "AuthUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateRegistrations_BusinessEntityId",
                table: "CertificateRegistrations",
                column: "BusinessEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateRegistrations_HEUserId",
                table: "CertificateRegistrations",
                column: "HEUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CertificateRegistrations");
        }
    }
}
