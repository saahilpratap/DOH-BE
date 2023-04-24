using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedTableControlRequirements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Domains",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    AuthoritativeDocumentName = table.Column<string>(nullable: true),
                    AuthoritativeDocumentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Domains_AuthoritativeDocuments_AuthoritativeDocumentId",
                        column: x => x.AuthoritativeDocumentId,
                        principalTable: "AuthoritativeDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ControlStandards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(nullable: false),
                    OriginalControlId = table.Column<string>(nullable: true),
                    DomainName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    AuthoritativeDocumentId = table.Column<int>(nullable: false),
                    DomainId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlStandards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ControlStandards_Domains_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ControlRequirements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    OriginalId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ControlStandardName = table.Column<string>(nullable: true),
                    DomainName = table.Column<string>(nullable: true),
                    ControlType = table.Column<int>(nullable: false),
                    ControlStandardId = table.Column<int>(nullable: false),
                    AuthoritativeDocumentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlRequirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ControlRequirements_ControlStandards_ControlStandardId",
                        column: x => x.ControlStandardId,
                        principalTable: "ControlStandards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequirementQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ControlRequirementId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequirementQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequirementQuestion_ControlRequirements_ControlRequirementId",
                        column: x => x.ControlRequirementId,
                        principalTable: "ControlRequirements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ControlRequirements_ControlStandardId",
                table: "ControlRequirements",
                column: "ControlStandardId");

            migrationBuilder.CreateIndex(
                name: "IX_ControlStandards_DomainId",
                table: "ControlStandards",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Domains_AuthoritativeDocumentId",
                table: "Domains",
                column: "AuthoritativeDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_RequirementQuestion_ControlRequirementId",
                table: "RequirementQuestion",
                column: "ControlRequirementId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequirementQuestion");

            migrationBuilder.DropTable(
                name: "ControlRequirements");

            migrationBuilder.DropTable(
                name: "ControlStandards");

            migrationBuilder.DropTable(
                name: "Domains");
        }
    }
}
