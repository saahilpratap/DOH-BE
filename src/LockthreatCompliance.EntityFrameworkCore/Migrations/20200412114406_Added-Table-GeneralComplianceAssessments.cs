using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedTableGeneralComplianceAssessments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GeneralComplianceAssessments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    SubmittedCount = table.Column<int>(nullable: false),
                    AllCount = table.Column<int>(nullable: false),
                    SubmittedPercentage = table.Column<double>(nullable: false),
                    ApprovedPercentage = table.Column<double>(nullable: false),
                    ApprovedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralComplianceAssessments", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralComplianceAssessments");
        }
    }
}
