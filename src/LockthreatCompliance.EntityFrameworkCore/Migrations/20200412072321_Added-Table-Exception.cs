using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedTableException : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exceptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    ExceptionTypeId = table.Column<int>(nullable: false),
                    ReviewStatus = table.Column<int>(nullable: false),
                    RequestorId = table.Column<long>(nullable: false),
                    BusinessEntityId = table.Column<int>(nullable: true),
                    RequestDate = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    NextReviewDate = table.Column<DateTime>(nullable: false),
                    ApprovedTillDate = table.Column<DateTime>(nullable: false),
                    ReviewComment = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    ExpertReviewerId = table.Column<long>(nullable: true),
                    ApproverId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exceptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exceptions_AbpUsers_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Exceptions_BusinessEntities_BusinessEntityId",
                        column: x => x.BusinessEntityId,
                        principalTable: "BusinessEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Exceptions_ExceptionTypes_ExceptionTypeId",
                        column: x => x.ExceptionTypeId,
                        principalTable: "ExceptionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Exceptions_AbpUsers_ExpertReviewerId",
                        column: x => x.ExpertReviewerId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Exceptions_AbpUsers_RequestorId",
                        column: x => x.RequestorId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExceptionCompensatingControls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExceptionId = table.Column<int>(nullable: false),
                    ControlRequirementId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionCompensatingControls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExceptionCompensatingControls_ControlRequirements_ControlRequirementId",
                        column: x => x.ControlRequirementId,
                        principalTable: "ControlRequirements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExceptionCompensatingControls_Exceptions_ExceptionId",
                        column: x => x.ExceptionId,
                        principalTable: "Exceptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExceptionImpactedControlRequirements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExceptionId = table.Column<int>(nullable: false),
                    ControlRequirementId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionImpactedControlRequirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExceptionImpactedControlRequirements_ControlRequirements_ControlRequirementId",
                        column: x => x.ControlRequirementId,
                        principalTable: "ControlRequirements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExceptionImpactedControlRequirements_Exceptions_ExceptionId",
                        column: x => x.ExceptionId,
                        principalTable: "Exceptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExceptionRelatedBusinessRisks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExceptionId = table.Column<int>(nullable: false),
                    BusinessRiskId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionRelatedBusinessRisks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExceptionRelatedBusinessRisks_BusinessRisks_BusinessRiskId",
                        column: x => x.BusinessRiskId,
                        principalTable: "BusinessRisks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExceptionRelatedBusinessRisks_Exceptions_ExceptionId",
                        column: x => x.ExceptionId,
                        principalTable: "Exceptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionCompensatingControls_ControlRequirementId",
                table: "ExceptionCompensatingControls",
                column: "ControlRequirementId");

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionCompensatingControls_ExceptionId",
                table: "ExceptionCompensatingControls",
                column: "ExceptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionImpactedControlRequirements_ControlRequirementId",
                table: "ExceptionImpactedControlRequirements",
                column: "ControlRequirementId");

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionImpactedControlRequirements_ExceptionId",
                table: "ExceptionImpactedControlRequirements",
                column: "ExceptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionRelatedBusinessRisks_BusinessRiskId",
                table: "ExceptionRelatedBusinessRisks",
                column: "BusinessRiskId");

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionRelatedBusinessRisks_ExceptionId",
                table: "ExceptionRelatedBusinessRisks",
                column: "ExceptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Exceptions_ApproverId",
                table: "Exceptions",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_Exceptions_BusinessEntityId",
                table: "Exceptions",
                column: "BusinessEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Exceptions_ExceptionTypeId",
                table: "Exceptions",
                column: "ExceptionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Exceptions_ExpertReviewerId",
                table: "Exceptions",
                column: "ExpertReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_Exceptions_RequestorId",
                table: "Exceptions",
                column: "RequestorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExceptionCompensatingControls");

            migrationBuilder.DropTable(
                name: "ExceptionImpactedControlRequirements");

            migrationBuilder.DropTable(
                name: "ExceptionRelatedBusinessRisks");

            migrationBuilder.DropTable(
                name: "Exceptions");
        }
    }
}
