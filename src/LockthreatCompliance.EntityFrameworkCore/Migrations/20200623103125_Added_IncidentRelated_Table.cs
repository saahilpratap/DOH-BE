using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class Added_IncidentRelated_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IncidentRelatedBusinessRisks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncidentId = table.Column<int>(nullable: true),
                    BusinessRiskId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentRelatedBusinessRisks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncidentRelatedBusinessRisks_BusinessRisks_BusinessRiskId",
                        column: x => x.BusinessRiskId,
                        principalTable: "BusinessRisks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncidentRelatedBusinessRisks_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncidentRelatedExceptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncidentId = table.Column<int>(nullable: true),
                    ExceptionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentRelatedExceptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncidentRelatedExceptions_Exceptions_ExceptionId",
                        column: x => x.ExceptionId,
                        principalTable: "Exceptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncidentRelatedExceptions_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncidentRelatedBusinessRisks_BusinessRiskId",
                table: "IncidentRelatedBusinessRisks",
                column: "BusinessRiskId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentRelatedBusinessRisks_IncidentId",
                table: "IncidentRelatedBusinessRisks",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentRelatedExceptions_ExceptionId",
                table: "IncidentRelatedExceptions",
                column: "ExceptionId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentRelatedExceptions_IncidentId",
                table: "IncidentRelatedExceptions",
                column: "IncidentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncidentRelatedBusinessRisks");

            migrationBuilder.DropTable(
                name: "IncidentRelatedExceptions");
        }
    }
}
